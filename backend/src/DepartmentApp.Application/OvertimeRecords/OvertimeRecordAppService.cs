using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.OvertimeRecords.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.OvertimeRecords
{
    public class OvertimeRecordAppService : AsyncCrudAppService<
        OvertimeRecord,
        OvertimeRecordDto,
        long,
        PagedOvertimeRecordResultRequestDto,
        CreateOvertimeRecordDto,
        OvertimeRecordDto>,
        IOvertimeRecordAppService
    {
        private readonly IRepository<StatusChangeLog, long> _statusChangeLogRepo;
        private readonly IRepository<ApprovalRecord, Guid> _approvalRepo;
        private readonly IFlowEngine _flowEngine;

        public OvertimeRecordAppService(IRepository<OvertimeRecord, long> repository, IFlowEngine flowEngine, IRepository<StatusChangeLog, long> statusChangeLogRepo, IRepository<ApprovalRecord, Guid> approvalRepo)
            : base(repository)
        {
            _flowEngine = flowEngine;
            _statusChangeLogRepo = statusChangeLogRepo;
            _approvalRepo = approvalRepo;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.OvertimeRecord_Read;
            GetAllPermissionName = PermissionNames.OvertimeRecord_Read;
            CreatePermissionName = PermissionNames.OvertimeRecord_Create;
            UpdatePermissionName = PermissionNames.OvertimeRecord_Update;
            DeletePermissionName = PermissionNames.OvertimeRecord_Delete;
        }

        protected override IQueryable<OvertimeRecord> CreateFilteredQuery(PagedOvertimeRecordResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Reason != null && x.Reason.Contains(input.Keyword)) ||
                    (x.ApproverNote != null && x.ApproverNote.Contains(input.Keyword)) ||
                    (x.Notes != null && x.Notes.Contains(input.Keyword)))
                .WhereIf(!input.Reason.IsNullOrWhiteSpace(), x => x.Reason != null && x.Reason.Contains(input.Reason))
                .WhereIf(!input.ApproverNote.IsNullOrWhiteSpace(), x => x.ApproverNote != null && x.ApproverNote.Contains(input.ApproverNote))
                .WhereIf(!input.Notes.IsNullOrWhiteSpace(), x => x.Notes != null && x.Notes.Contains(input.Notes))
                .WhereIf(input.OvertimeDate.HasValue, x => x.OvertimeDate == input.OvertimeDate.Value)
                .WhereIf(input.Hours.HasValue, x => x.Hours == input.Hours.Value)
                .WhereIf(input.Status.HasValue, x => x.Status == (Status)input.Status.Value)
                .WhereIf(input.ManagerApproverId.HasValue, x => x.ManagerApproverId == input.ManagerApproverId.Value)
                .WhereIf(input.EmployeeId.HasValue, x => x.EmployeeId == input.EmployeeId.Value);
        }

        public override async Task<OvertimeRecordDto> CreateAsync(CreateOvertimeRecordDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "OvertimeRecord", result);

            // Frontend creates records with status pre-set without going through ChangeStatusAsync,
            // so mirror on-field-change here whenever the initial status isn't the default. Otherwise
            // status-driven flows (e.g. approval) never fire on plain Create.
            if (result.Status != (int)Status.Pending)
                await _flowEngine.TriggerAsync("on-field-change", "OvertimeRecord", result);
            return result;
        }

        public override async Task<OvertimeRecordDto> UpdateAsync(OvertimeRecordDto input)
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
                    EntityType = "OvertimeRecord",
                    EntityId = input.Id.ToString(),
                    FromStatus = fromStatus,
                    ToStatus = toStatus,
                    Action = "Update",
                    ChangedByUserId = AbpSession.UserId
                });
            }

            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "OvertimeRecord", result);

            // Frontend updates status via plain UpdateAsync (not ChangeStatusAsync) — fire
            // on-field-change so status-driven flows pick up the transition.
            if (statusChanged)
                await _flowEngine.TriggerAsync("on-field-change", "OvertimeRecord", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "OvertimeRecord", new { Id = input.Id });
        }

        [Abp.Authorization.AbpAuthorize(PermissionNames.OvertimeRecord_Update)]
        public async Task<OvertimeRecordDto> ChangeStatusAsync(long id, ChangeStatusInput input)
        {
            var entity = await Repository.GetAsync(id);
            var currentStatus = entity.Status.ToString();

            // Find valid transition
            var transitions = new (string From, string To, string Action, bool Readonly)[]
            {
            ("Pending", "Approved", "Approve", false),
            ("Pending", "Rejected", "Reject", true),
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
                    .Where(a => a.EntityType == "OvertimeRecord" && a.EntityId == id.ToString() && a.Status == "Pending")
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
                EntityType = "OvertimeRecord",
                EntityId = id.ToString(),
                FromStatus = fromStatus,
                ToStatus = transition.To,
                Action = input.Action,
                Comment = input.ActionData != null && input.ActionData.ContainsKey("comment") ? input.ActionData["comment"] : null,
                ChangedByUserId = AbpSession.UserId
            });

            var result = MapToEntityDto(entity);

            // Trigger flow: on-status-change (always)
            await _flowEngine.TriggerAsync("on-field-change", "OvertimeRecord", result);

            // Trigger named flow events
            if (input.Action == "Approve")
                await _flowEngine.TriggerAsync("submit-for-approval", "OvertimeRecord", result);
            return result;
        }

        private void ValidateStatusTransition(Status from, Status to)
        {
            var allowed = new (string From, string To)[]
            {
                ("Pending", "Approved"),
                ("Pending", "Rejected"),
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
