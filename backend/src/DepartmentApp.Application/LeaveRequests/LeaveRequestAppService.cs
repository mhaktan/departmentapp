using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.LeaveRequests.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.LeaveRequests
{
    public class LeaveRequestAppService : AsyncCrudAppService<
        LeaveRequest,
        LeaveRequestDto,
        long,
        PagedLeaveRequestResultRequestDto,
        CreateLeaveRequestDto,
        LeaveRequestDto>,
        ILeaveRequestAppService
    {
        private readonly IRepository<StatusChangeLog, long> _statusChangeLogRepo;
        private readonly IRepository<ApprovalRecord, Guid> _approvalRepo;
        private readonly IFlowEngine _flowEngine;

        public LeaveRequestAppService(IRepository<LeaveRequest, long> repository, IFlowEngine flowEngine, IRepository<StatusChangeLog, long> statusChangeLogRepo, IRepository<ApprovalRecord, Guid> approvalRepo)
            : base(repository)
        {
            _flowEngine = flowEngine;
            _statusChangeLogRepo = statusChangeLogRepo;
            _approvalRepo = approvalRepo;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.LeaveRequest_Read;
            GetAllPermissionName = PermissionNames.LeaveRequest_Read;
            CreatePermissionName = PermissionNames.LeaveRequest_Create;
            UpdatePermissionName = PermissionNames.LeaveRequest_Update;
            DeletePermissionName = PermissionNames.LeaveRequest_Delete;
        }

        protected override IQueryable<LeaveRequest> CreateFilteredQuery(PagedLeaveRequestResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Reason != null && x.Reason.Contains(input.Keyword)) ||
                    (x.RevisionNote != null && x.RevisionNote.Contains(input.Keyword)))
                .WhereIf(!input.Reason.IsNullOrWhiteSpace(), x => x.Reason != null && x.Reason.Contains(input.Reason))
                .WhereIf(!input.RevisionNote.IsNullOrWhiteSpace(), x => x.RevisionNote != null && x.RevisionNote.Contains(input.RevisionNote))
                .WhereIf(input.StartDate.HasValue, x => x.StartDate == input.StartDate.Value)
                .WhereIf(input.EndDate.HasValue, x => x.EndDate == input.EndDate.Value)
                .WhereIf(input.TotalDays.HasValue, x => x.TotalDays == input.TotalDays.Value)
                .WhereIf(input.Status.HasValue, x => x.Status == (Status)input.Status.Value)
                .WhereIf(input.RequiresHRApproval.HasValue, x => x.RequiresHRApproval == input.RequiresHRApproval.Value)
                .WhereIf(input.ManagerApproverId.HasValue, x => x.ManagerApproverId == input.ManagerApproverId.Value)
                .WhereIf(input.HrApproverId.HasValue, x => x.HrApproverId == input.HrApproverId.Value)
                .WhereIf(input.BalanceDeducted.HasValue, x => x.BalanceDeducted == input.BalanceDeducted.Value)
                .WhereIf(input.EmployeeId.HasValue, x => x.EmployeeId == input.EmployeeId.Value)
                .WhereIf(input.LeaveTypeId.HasValue, x => x.LeaveTypeId == input.LeaveTypeId.Value);
        }

        public override async Task<LeaveRequestDto> CreateAsync(CreateLeaveRequestDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "LeaveRequest", result);

            // Frontend creates records with status pre-set without going through ChangeStatusAsync,
            // so mirror on-field-change here whenever the initial status isn't the default. Otherwise
            // status-driven flows (e.g. approval) never fire on plain Create.
            if (result.Status != (int)Status.Draft)
                await _flowEngine.TriggerAsync("on-field-change", "LeaveRequest", result);
            return result;
        }

        public override async Task<LeaveRequestDto> UpdateAsync(LeaveRequestDto input)
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
                    EntityType = "LeaveRequest",
                    EntityId = input.Id.ToString(),
                    FromStatus = fromStatus,
                    ToStatus = toStatus,
                    Action = "Update",
                    ChangedByUserId = AbpSession.UserId
                });
            }

            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "LeaveRequest", result);

            // Frontend updates status via plain UpdateAsync (not ChangeStatusAsync) — fire
            // on-field-change so status-driven flows pick up the transition.
            if (statusChanged)
                await _flowEngine.TriggerAsync("on-field-change", "LeaveRequest", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "LeaveRequest", new { Id = input.Id });
        }

        [Abp.Authorization.AbpAuthorize(PermissionNames.LeaveRequest_Update)]
        public async Task<LeaveRequestDto> ChangeStatusAsync(long id, ChangeStatusInput input)
        {
            var entity = await Repository.GetAsync(id);
            var currentStatus = entity.Status.ToString();

            // Find valid transition
            var transitions = new (string From, string To, string Action, bool Readonly)[]
            {
            ("Draft", "PendingManagerApproval", "Submit", false),
            ("PendingManagerApproval", "PendingHRApproval", "Approve", false),
            ("PendingManagerApproval", "Revision", "Revise", false),
            ("PendingManagerApproval", "Rejected", "Reject", true),
            ("PendingHRApproval", "Approved", "Approve", false),
            ("PendingHRApproval", "Revision", "Revise", false),
            ("PendingHRApproval", "Rejected", "Reject", true),
            ("Revision", "PendingManagerApproval", "Resubmit", false),
            ("*", "Cancelled", "Cancel", true)
            };

            var transition = transitions.FirstOrDefault(t =>
                (t.From == "*" || t.From == currentStatus) && t.Action == input.Action);

            if (transition == default)
                throw new Abp.UI.UserFriendlyException($"Invalid action '{input.Action}' from status '{currentStatus}'");

            // Validate required fields per transition
            if (input.Action == "Revise" && (input.ActionData == null || !input.ActionData.ContainsKey("revisionNote") || string.IsNullOrWhiteSpace(input.ActionData["revisionNote"])))
                throw new Abp.UI.UserFriendlyException("Revise requires: revisionNote");

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
                    .Where(a => a.EntityType == "LeaveRequest" && a.EntityId == id.ToString() && a.Status == "Pending")
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
                EntityType = "LeaveRequest",
                EntityId = id.ToString(),
                FromStatus = fromStatus,
                ToStatus = transition.To,
                Action = input.Action,
                Comment = input.ActionData != null && input.ActionData.ContainsKey("comment") ? input.ActionData["comment"] : null,
                ChangedByUserId = AbpSession.UserId
            });

            var result = MapToEntityDto(entity);

            // Trigger flow: on-status-change (always)
            await _flowEngine.TriggerAsync("on-field-change", "LeaveRequest", result);

            // Trigger named flow events
            if (input.Action == "Submit")
                await _flowEngine.TriggerAsync("submit-for-approval", "LeaveRequest", result);
            return result;
        }

        private void ValidateStatusTransition(Status from, Status to)
        {
            var allowed = new (string From, string To)[]
            {
                ("Draft", "PendingManagerApproval"),
                ("PendingManagerApproval", "PendingHRApproval"),
                ("PendingManagerApproval", "Revision"),
                ("PendingManagerApproval", "Rejected"),
                ("PendingHRApproval", "Approved"),
                ("PendingHRApproval", "Revision"),
                ("PendingHRApproval", "Rejected"),
                ("Revision", "PendingManagerApproval"),
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
