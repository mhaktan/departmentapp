using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.DisciplinaryRecords.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.DisciplinaryRecords
{
    public class DisciplinaryRecordAppService : AsyncCrudAppService<
        DisciplinaryRecord,
        DisciplinaryRecordDto,
        long,
        PagedDisciplinaryRecordResultRequestDto,
        CreateDisciplinaryRecordDto,
        DisciplinaryRecordDto>,
        IDisciplinaryRecordAppService
    {
        private readonly IRepository<StatusChangeLog, long> _statusChangeLogRepo;
        private readonly IRepository<ApprovalRecord, Guid> _approvalRepo;
        private readonly IFlowEngine _flowEngine;

        public DisciplinaryRecordAppService(IRepository<DisciplinaryRecord, long> repository, IFlowEngine flowEngine, IRepository<StatusChangeLog, long> statusChangeLogRepo, IRepository<ApprovalRecord, Guid> approvalRepo)
            : base(repository)
        {
            _flowEngine = flowEngine;
            _statusChangeLogRepo = statusChangeLogRepo;
            _approvalRepo = approvalRepo;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.DisciplinaryRecord_Read;
            GetAllPermissionName = PermissionNames.DisciplinaryRecord_Read;
            CreatePermissionName = PermissionNames.DisciplinaryRecord_Create;
            UpdatePermissionName = PermissionNames.DisciplinaryRecord_Update;
            DeletePermissionName = PermissionNames.DisciplinaryRecord_Delete;
        }

        protected override IQueryable<DisciplinaryRecord> CreateFilteredQuery(PagedDisciplinaryRecordResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Description != null && x.Description.Contains(input.Keyword)) ||
                    (x.ActionTaken != null && x.ActionTaken.Contains(input.Keyword)) ||
                    (x.IssuedBy != null && x.IssuedBy.Contains(input.Keyword)) ||
                    (x.AppealNote != null && x.AppealNote.Contains(input.Keyword)) ||
                    (x.ResolutionNote != null && x.ResolutionNote.Contains(input.Keyword)))
                .WhereIf(!input.Description.IsNullOrWhiteSpace(), x => x.Description != null && x.Description.Contains(input.Description))
                .WhereIf(!input.ActionTaken.IsNullOrWhiteSpace(), x => x.ActionTaken != null && x.ActionTaken.Contains(input.ActionTaken))
                .WhereIf(!input.IssuedBy.IsNullOrWhiteSpace(), x => x.IssuedBy != null && x.IssuedBy.Contains(input.IssuedBy))
                .WhereIf(!input.AppealNote.IsNullOrWhiteSpace(), x => x.AppealNote != null && x.AppealNote.Contains(input.AppealNote))
                .WhereIf(!input.ResolutionNote.IsNullOrWhiteSpace(), x => x.ResolutionNote != null && x.ResolutionNote.Contains(input.ResolutionNote))
                .WhereIf(input.IncidentDate.HasValue, x => x.IncidentDate == input.IncidentDate.Value)
                .WhereIf(input.Type.HasValue, x => x.Type == (DisciplinaryRecordType)input.Type.Value)
                .WhereIf(input.AcknowledgedByEmployee.HasValue, x => x.AcknowledgedByEmployee == input.AcknowledgedByEmployee.Value)
                .WhereIf(input.Status.HasValue, x => x.Status == (DisciplinaryRecordStatus)input.Status.Value)
                .WhereIf(input.HrReviewerId.HasValue, x => x.HrReviewerId == input.HrReviewerId.Value)
                .WhereIf(input.HrManagerResolverId.HasValue, x => x.HrManagerResolverId == input.HrManagerResolverId.Value)
                .WhereIf(input.EmployeeId.HasValue, x => x.EmployeeId == input.EmployeeId.Value);
        }

        public override async Task<DisciplinaryRecordDto> CreateAsync(CreateDisciplinaryRecordDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "DisciplinaryRecord", result);

            // Frontend creates records with status pre-set without going through ChangeStatusAsync,
            // so mirror on-field-change here whenever the initial status isn't the default. Otherwise
            // status-driven flows (e.g. approval) never fire on plain Create.
            if (result.Status != (int)DisciplinaryRecordStatus.Open)
                await _flowEngine.TriggerAsync("on-field-change", "DisciplinaryRecord", result);
            return result;
        }

        public override async Task<DisciplinaryRecordDto> UpdateAsync(DisciplinaryRecordDto input)
        {
            // State machine: validate status transition + log
            var existing = await Repository.GetAsync(input.Id);
            var statusChanged = (int)existing.Status != input.Status;
            if (statusChanged)
            {
                var fromStatus = existing.Status.ToString();
                var toStatus = ((DisciplinaryRecordStatus)input.Status).ToString();
                ValidateStatusTransition(existing.Status, (DisciplinaryRecordStatus)input.Status);

                // Log status change
                await _statusChangeLogRepo.InsertAsync(new StatusChangeLog
                {
                    EntityType = "DisciplinaryRecord",
                    EntityId = input.Id.ToString(),
                    FromStatus = fromStatus,
                    ToStatus = toStatus,
                    Action = "Update",
                    ChangedByUserId = AbpSession.UserId
                });
            }

            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "DisciplinaryRecord", result);

            // Frontend updates status via plain UpdateAsync (not ChangeStatusAsync) — fire
            // on-field-change so status-driven flows pick up the transition.
            if (statusChanged)
                await _flowEngine.TriggerAsync("on-field-change", "DisciplinaryRecord", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "DisciplinaryRecord", new { Id = input.Id });
        }

        [Abp.Authorization.AbpAuthorize(PermissionNames.DisciplinaryRecord_Update)]
        public async Task<DisciplinaryRecordDto> ChangeStatusAsync(long id, ChangeStatusInput input)
        {
            var entity = await Repository.GetAsync(id);
            var currentStatus = entity.Status.ToString();

            // Find valid transition
            var transitions = new (string From, string To, string Action, bool Readonly)[]
            {
            ("Open", "UnderReview", "StartReview", false),
            ("UnderReview", "Resolved", "Resolve", false),
            ("UnderReview", "Appealed", "Appeal", false),
            ("Appealed", "UnderReview", "Reopen", false),
            ("Resolved", "Closed", "Close", true),
            ("Appealed", "Closed", "Close", true)
            };

            var transition = transitions.FirstOrDefault(t =>
                (t.From == "*" || t.From == currentStatus) && t.Action == input.Action);

            if (transition == default)
                throw new Abp.UI.UserFriendlyException($"Invalid action '{input.Action}' from status '{currentStatus}'");

            // Validate required fields per transition
            if (input.Action == "Resolve" && (input.ActionData == null || !input.ActionData.ContainsKey("resolutionNote") || string.IsNullOrWhiteSpace(input.ActionData["resolutionNote"])))
                throw new Abp.UI.UserFriendlyException("Resolve requires: resolutionNote");
            if (input.Action == "Appeal" && (input.ActionData == null || !input.ActionData.ContainsKey("appealNote") || string.IsNullOrWhiteSpace(input.ActionData["appealNote"])))
                throw new Abp.UI.UserFriendlyException("Appeal requires: appealNote");

            var fromStatus = currentStatus;

            // Apply new status
            entity.Status = (DisciplinaryRecordStatus)Enum.Parse(typeof(DisciplinaryRecordStatus), transition.To);
            await Repository.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            // Cancel pending ApprovalRecords when the entity is cancelled — otherwise the records
            // sit forever in approvers' inboxes pointing to a cancelled request.
            if (input.Action == "Cancel")
            {
                var pending = _approvalRepo.GetAll()
                    .Where(a => a.EntityType == "DisciplinaryRecord" && a.EntityId == id.ToString() && a.Status == "Pending")
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
                EntityType = "DisciplinaryRecord",
                EntityId = id.ToString(),
                FromStatus = fromStatus,
                ToStatus = transition.To,
                Action = input.Action,
                Comment = input.ActionData != null && input.ActionData.ContainsKey("comment") ? input.ActionData["comment"] : null,
                ChangedByUserId = AbpSession.UserId
            });

            var result = MapToEntityDto(entity);

            // Trigger flow: on-status-change (always)
            await _flowEngine.TriggerAsync("on-field-change", "DisciplinaryRecord", result);

            return result;
        }

        private void ValidateStatusTransition(DisciplinaryRecordStatus from, DisciplinaryRecordStatus to)
        {
            var allowed = new (string From, string To)[]
            {
                ("Open", "UnderReview"),
                ("UnderReview", "Resolved"),
                ("UnderReview", "Appealed"),
                ("Appealed", "UnderReview"),
                ("Resolved", "Closed"),
                ("Appealed", "Closed")
            };

            var isValid = allowed.Any(t =>
                (t.From == "*" || t.From == from.ToString()) &&
                t.To == to.ToString());

            if (!isValid)
                throw new Abp.UI.UserFriendlyException($"Invalid status transition from {from} to {to}");
        }
    }
}
