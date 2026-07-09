using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.Trainings.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.Trainings
{
    public class TrainingAppService : AsyncCrudAppService<
        Training,
        TrainingDto,
        long,
        PagedTrainingResultRequestDto,
        CreateTrainingDto,
        TrainingDto>,
        ITrainingAppService
    {
        private readonly IRepository<StatusChangeLog, long> _statusChangeLogRepo;
        private readonly IRepository<ApprovalRecord, Guid> _approvalRepo;
        private readonly IFlowEngine _flowEngine;

        public TrainingAppService(IRepository<Training, long> repository, IFlowEngine flowEngine, IRepository<StatusChangeLog, long> statusChangeLogRepo, IRepository<ApprovalRecord, Guid> approvalRepo)
            : base(repository)
        {
            _flowEngine = flowEngine;
            _statusChangeLogRepo = statusChangeLogRepo;
            _approvalRepo = approvalRepo;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.Training_Read;
            GetAllPermissionName = PermissionNames.Training_Read;
            CreatePermissionName = PermissionNames.Training_Create;
            UpdatePermissionName = PermissionNames.Training_Update;
            DeletePermissionName = PermissionNames.Training_Delete;
        }

        protected override IQueryable<Training> CreateFilteredQuery(PagedTrainingResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Title != null && x.Title.Contains(input.Keyword)) ||
                    (x.Provider != null && x.Provider.Contains(input.Keyword)) ||
                    (x.Location != null && x.Location.Contains(input.Keyword)) ||
                    (x.Currency != null && x.Currency.Contains(input.Keyword)))
                .WhereIf(!input.Title.IsNullOrWhiteSpace(), x => x.Title != null && x.Title.Contains(input.Title))
                .WhereIf(!input.Provider.IsNullOrWhiteSpace(), x => x.Provider != null && x.Provider.Contains(input.Provider))
                .WhereIf(!input.Location.IsNullOrWhiteSpace(), x => x.Location != null && x.Location.Contains(input.Location))
                .WhereIf(!input.Currency.IsNullOrWhiteSpace(), x => x.Currency != null && x.Currency.Contains(input.Currency))
                .WhereIf(input.StartDate.HasValue, x => x.StartDate == input.StartDate.Value)
                .WhereIf(input.EndDate.HasValue, x => x.EndDate == input.EndDate.Value)
                .WhereIf(input.TrainingType.HasValue, x => x.TrainingType == (TrainingTrainingType)input.TrainingType.Value)
                .WhereIf(input.Status.HasValue, x => x.Status == (TrainingStatus)input.Status.Value)
                .WhereIf(input.Cost.HasValue, x => x.Cost == input.Cost.Value)
                .WhereIf(input.TrainingPlanId.HasValue, x => x.TrainingPlanId == input.TrainingPlanId.Value);
        }

        public override async Task<TrainingDto> CreateAsync(CreateTrainingDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "Training", result);

            // Frontend creates records with status pre-set without going through ChangeStatusAsync,
            // so mirror on-field-change here whenever the initial status isn't the default. Otherwise
            // status-driven flows (e.g. approval) never fire on plain Create.
            if (result.Status != (int)TrainingStatus.Planned)
                await _flowEngine.TriggerAsync("on-field-change", "Training", result);
            return result;
        }

        public override async Task<TrainingDto> UpdateAsync(TrainingDto input)
        {
            // State machine: validate status transition + log
            var existing = await Repository.GetAsync(input.Id);
            var statusChanged = (int)existing.Status != input.Status;
            if (statusChanged)
            {
                var fromStatus = existing.Status.ToString();
                var toStatus = ((TrainingStatus)input.Status).ToString();
                ValidateStatusTransition(existing.Status, (TrainingStatus)input.Status);

                // Log status change
                await _statusChangeLogRepo.InsertAsync(new StatusChangeLog
                {
                    EntityType = "Training",
                    EntityId = input.Id.ToString(),
                    FromStatus = fromStatus,
                    ToStatus = toStatus,
                    Action = "Update",
                    ChangedByUserId = AbpSession.UserId
                });
            }

            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "Training", result);

            // Frontend updates status via plain UpdateAsync (not ChangeStatusAsync) — fire
            // on-field-change so status-driven flows pick up the transition.
            if (statusChanged)
                await _flowEngine.TriggerAsync("on-field-change", "Training", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "Training", new { Id = input.Id });
        }

        [Abp.Authorization.AbpAuthorize(PermissionNames.Training_Update)]
        public async Task<TrainingDto> ChangeStatusAsync(long id, ChangeStatusInput input)
        {
            var entity = await Repository.GetAsync(id);
            var currentStatus = entity.Status.ToString();

            // Find valid transition
            var transitions = new (string From, string To, string Action, bool Readonly)[]
            {
            ("Planned", "Ongoing", "Start", false),
            ("Ongoing", "Completed", "Complete", true),
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
            entity.Status = (TrainingStatus)Enum.Parse(typeof(TrainingStatus), transition.To);
            await Repository.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            // Cancel pending ApprovalRecords when the entity is cancelled — otherwise the records
            // sit forever in approvers' inboxes pointing to a cancelled request.
            if (input.Action == "Cancel")
            {
                var pending = _approvalRepo.GetAll()
                    .Where(a => a.EntityType == "Training" && a.EntityId == id.ToString() && a.Status == "Pending")
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
                EntityType = "Training",
                EntityId = id.ToString(),
                FromStatus = fromStatus,
                ToStatus = transition.To,
                Action = input.Action,
                Comment = input.ActionData != null && input.ActionData.ContainsKey("comment") ? input.ActionData["comment"] : null,
                ChangedByUserId = AbpSession.UserId
            });

            var result = MapToEntityDto(entity);

            // Trigger flow: on-status-change (always)
            await _flowEngine.TriggerAsync("on-field-change", "Training", result);

            return result;
        }

        private void ValidateStatusTransition(TrainingStatus from, TrainingStatus to)
        {
            var allowed = new (string From, string To)[]
            {
                ("Planned", "Ongoing"),
                ("Ongoing", "Completed"),
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
