using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.Onboardings.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.Onboardings
{
    public class OnboardingAppService : AsyncCrudAppService<
        Onboarding,
        OnboardingDto,
        long,
        PagedOnboardingResultRequestDto,
        CreateOnboardingDto,
        OnboardingDto>,
        IOnboardingAppService
    {
        private readonly IRepository<StatusChangeLog, long> _statusChangeLogRepo;
        private readonly IRepository<ApprovalRecord, Guid> _approvalRepo;
        private readonly IFlowEngine _flowEngine;

        public OnboardingAppService(IRepository<Onboarding, long> repository, IFlowEngine flowEngine, IRepository<StatusChangeLog, long> statusChangeLogRepo, IRepository<ApprovalRecord, Guid> approvalRepo)
            : base(repository)
        {
            _flowEngine = flowEngine;
            _statusChangeLogRepo = statusChangeLogRepo;
            _approvalRepo = approvalRepo;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.Onboarding_Read;
            GetAllPermissionName = PermissionNames.Onboarding_Read;
            CreatePermissionName = PermissionNames.Onboarding_Create;
            UpdatePermissionName = PermissionNames.Onboarding_Update;
            DeletePermissionName = PermissionNames.Onboarding_Delete;
        }

        protected override IQueryable<Onboarding> CreateFilteredQuery(PagedOnboardingResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Notes != null && x.Notes.Contains(input.Keyword)))
                .WhereIf(!input.Notes.IsNullOrWhiteSpace(), x => x.Notes != null && x.Notes.Contains(input.Notes))
                .WhereIf(input.StartDate.HasValue, x => x.StartDate == input.StartDate.Value)
                .WhereIf(input.ExpectedCompletionDate.HasValue, x => x.ExpectedCompletionDate == input.ExpectedCompletionDate.Value)
                .WhereIf(input.Status.HasValue, x => x.Status == (Status)input.Status.Value)
                .WhereIf(input.JobApplicationId.HasValue, x => x.JobApplicationId == input.JobApplicationId.Value);
        }

        public override async Task<OnboardingDto> CreateAsync(CreateOnboardingDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "Onboarding", result);

            // Frontend creates records with status pre-set without going through ChangeStatusAsync,
            // so mirror on-field-change here whenever the initial status isn't the default. Otherwise
            // status-driven flows (e.g. approval) never fire on plain Create.
            if (result.Status != (int)Status.NotStarted)
                await _flowEngine.TriggerAsync("on-field-change", "Onboarding", result);
            return result;
        }

        public override async Task<OnboardingDto> UpdateAsync(OnboardingDto input)
        {
            // State machine: validate status transition + log
            var existing = await Repository.GetAsync(input.Id);
            var statusChanged = (int)existing.Status != input.Status;
            if (statusChanged)
            {
                var fromStatus = existing.Status.ToString();
                var toStatus = ((Status)input.Status).ToString();
                ValidateStatusTransition(existing.Status, (Status)input.Status);

                // Log status change
                await _statusChangeLogRepo.InsertAsync(new StatusChangeLog
                {
                    EntityType = "Onboarding",
                    EntityId = input.Id.ToString(),
                    FromStatus = fromStatus,
                    ToStatus = toStatus,
                    Action = "Update",
                    ChangedByUserId = AbpSession.UserId
                });
            }

            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "Onboarding", result);

            // Frontend updates status via plain UpdateAsync (not ChangeStatusAsync) — fire
            // on-field-change so status-driven flows pick up the transition.
            if (statusChanged)
                await _flowEngine.TriggerAsync("on-field-change", "Onboarding", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "Onboarding", new { Id = input.Id });
        }

        [Abp.Authorization.AbpAuthorize(PermissionNames.Onboarding_Update)]
        public async Task<OnboardingDto> ChangeStatusAsync(long id, ChangeStatusInput input)
        {
            var entity = await Repository.GetAsync(id);
            var currentStatus = entity.Status.ToString();

            // Find valid transition
            var transitions = new (string From, string To, string Action, bool Readonly)[]
            {
            ("NotStarted", "InProgress", "Start", false),
            ("InProgress", "Completed", "Complete", true),
            ("*", "Cancelled", "Cancel", true)
            };

            var transition = transitions.FirstOrDefault(t =>
                (t.From == "*" || t.From == currentStatus) && t.Action == input.Action);

            if (transition == default)
                throw new Abp.UI.UserFriendlyException($"Invalid action '{input.Action}' from status '{currentStatus}'");

            // Validate required fields per transition
            // No required fields for any transition

            var fromStatus = currentStatus;

            // Apply new status
            entity.Status = (Status)Enum.Parse(typeof(Status), transition.To);
            await Repository.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            // Cancel pending ApprovalRecords when the entity is cancelled — otherwise the records
            // sit forever in approvers' inboxes pointing to a cancelled request.
            if (input.Action == "Cancel")
            {
                var pending = _approvalRepo.GetAll()
                    .Where(a => a.EntityType == "Onboarding" && a.EntityId == id.ToString() && a.Status == "Pending")
                    .ToList();
                foreach (var pendingRec in pending)
                {
                    pendingRec.Status = "Cancelled";
                    pendingRec.ActionTaken = "Cancel";
                    pendingRec.ActionDate = DateTime.UtcNow;
                    pendingRec.Comment = "Entity cancelled by submitter.";
                    await _approvalRepo.UpdateAsync(pendingRec);
                }
            }

            // Log status change
            await _statusChangeLogRepo.InsertAsync(new Entities.StatusChangeLog
            {
                EntityType = "Onboarding",
                EntityId = id.ToString(),
                FromStatus = fromStatus,
                ToStatus = transition.To,
                Action = input.Action,
                Comment = input.ActionData != null && input.ActionData.ContainsKey("comment") ? input.ActionData["comment"] : null,
                ChangedByUserId = AbpSession.UserId
            });

            var result = MapToEntityDto(entity);

            // Trigger flow: on-status-change (always)
            await _flowEngine.TriggerAsync("on-field-change", "Onboarding", result);

            return result;
        }

        private void ValidateStatusTransition(Status from, Status to)
        {
            var allowed = new (string From, string To)[]
            {
                ("NotStarted", "InProgress"),
                ("InProgress", "Completed"),
                ("*", "Cancelled")
            };

            var isValid = allowed.Any(t =>
                (t.From == "*" || t.From == from.ToString()) &&
                t.To == to.ToString());

            if (!isValid)
                throw new Abp.UI.UserFriendlyException($"Invalid status transition from {from} to {to}");
        }
    }
}
