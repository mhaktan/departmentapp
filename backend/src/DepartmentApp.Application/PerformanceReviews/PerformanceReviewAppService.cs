using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.PerformanceReviews.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.PerformanceReviews
{
    public class PerformanceReviewAppService : AsyncCrudAppService<
        PerformanceReview,
        PerformanceReviewDto,
        long,
        PagedPerformanceReviewResultRequestDto,
        CreatePerformanceReviewDto,
        PerformanceReviewDto>,
        IPerformanceReviewAppService
    {
        private readonly IRepository<StatusChangeLog, long> _statusChangeLogRepo;
        private readonly IRepository<ApprovalRecord, Guid> _approvalRepo;
        private readonly IFlowEngine _flowEngine;

        public PerformanceReviewAppService(IRepository<PerformanceReview, long> repository, IFlowEngine flowEngine, IRepository<StatusChangeLog, long> statusChangeLogRepo, IRepository<ApprovalRecord, Guid> approvalRepo)
            : base(repository)
        {
            _flowEngine = flowEngine;
            _statusChangeLogRepo = statusChangeLogRepo;
            _approvalRepo = approvalRepo;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.PerformanceReview_Read;
            GetAllPermissionName = PermissionNames.PerformanceReview_Read;
            CreatePermissionName = PermissionNames.PerformanceReview_Create;
            UpdatePermissionName = PermissionNames.PerformanceReview_Update;
            DeletePermissionName = PermissionNames.PerformanceReview_Delete;
        }

        protected override IQueryable<PerformanceReview> CreateFilteredQuery(PagedPerformanceReviewResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.ReviewPeriod != null && x.ReviewPeriod.Contains(input.Keyword)) ||
                    (x.SelfAssessmentNotes != null && x.SelfAssessmentNotes.Contains(input.Keyword)) ||
                    (x.ManagerNotes != null && x.ManagerNotes.Contains(input.Keyword)) ||
                    (x.HrNotes != null && x.HrNotes.Contains(input.Keyword)) ||
                    (x.RevisionNote != null && x.RevisionNote.Contains(input.Keyword)))
                .WhereIf(!input.ReviewPeriod.IsNullOrWhiteSpace(), x => x.ReviewPeriod != null && x.ReviewPeriod.Contains(input.ReviewPeriod))
                .WhereIf(!input.SelfAssessmentNotes.IsNullOrWhiteSpace(), x => x.SelfAssessmentNotes != null && x.SelfAssessmentNotes.Contains(input.SelfAssessmentNotes))
                .WhereIf(!input.ManagerNotes.IsNullOrWhiteSpace(), x => x.ManagerNotes != null && x.ManagerNotes.Contains(input.ManagerNotes))
                .WhereIf(!input.HrNotes.IsNullOrWhiteSpace(), x => x.HrNotes != null && x.HrNotes.Contains(input.HrNotes))
                .WhereIf(!input.RevisionNote.IsNullOrWhiteSpace(), x => x.RevisionNote != null && x.RevisionNote.Contains(input.RevisionNote))
                .WhereIf(input.ReviewYear.HasValue, x => x.ReviewYear == input.ReviewYear.Value)
                .WhereIf(input.ReviewType.HasValue, x => x.ReviewType == (PerformanceReviewReviewType)input.ReviewType.Value)
                .WhereIf(input.Status.HasValue, x => x.Status == (PerformanceReviewStatus)input.Status.Value)
                .WhereIf(input.SelfAssessmentScore.HasValue, x => x.SelfAssessmentScore == input.SelfAssessmentScore.Value)
                .WhereIf(input.ManagerScore.HasValue, x => x.ManagerScore == input.ManagerScore.Value)
                .WhereIf(input.OverallScore.HasValue, x => x.OverallScore == input.OverallScore.Value)
                .WhereIf(input.ManagerReviewerId.HasValue, x => x.ManagerReviewerId == input.ManagerReviewerId.Value)
                .WhereIf(input.HrReviewerId.HasValue, x => x.HrReviewerId == input.HrReviewerId.Value)
                .WhereIf(input.PeerReviewersAssignedBy.HasValue, x => x.PeerReviewersAssignedBy == input.PeerReviewersAssignedBy.Value)
                .WhereIf(input.EmployeeId.HasValue, x => x.EmployeeId == input.EmployeeId.Value);
        }

        public override async Task<PerformanceReviewDto> CreateAsync(CreatePerformanceReviewDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "PerformanceReview", result);

            // Frontend creates records with status pre-set without going through ChangeStatusAsync,
            // so mirror on-field-change here whenever the initial status isn't the default. Otherwise
            // status-driven flows (e.g. approval) never fire on plain Create.
            if (result.Status != (int)PerformanceReviewStatus.Draft)
                await _flowEngine.TriggerAsync("on-field-change", "PerformanceReview", result);
            return result;
        }

        public override async Task<PerformanceReviewDto> UpdateAsync(PerformanceReviewDto input)
        {
            // State machine: validate status transition + log
            var existing = await Repository.GetAsync(input.Id);
            var statusChanged = (int)existing.Status != input.Status;
            if (statusChanged)
            {
                var fromStatus = existing.Status.ToString();
                var toStatus = ((PerformanceReviewStatus)input.Status).ToString();
                ValidateStatusTransition(existing.Status, (PerformanceReviewStatus)input.Status);

                // Log status change
                await _statusChangeLogRepo.InsertAsync(new StatusChangeLog
                {
                    EntityType = "PerformanceReview",
                    EntityId = input.Id.ToString(),
                    FromStatus = fromStatus,
                    ToStatus = toStatus,
                    Action = "Update",
                    ChangedByUserId = AbpSession.UserId
                });
            }

            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "PerformanceReview", result);

            // Frontend updates status via plain UpdateAsync (not ChangeStatusAsync) — fire
            // on-field-change so status-driven flows pick up the transition.
            if (statusChanged)
                await _flowEngine.TriggerAsync("on-field-change", "PerformanceReview", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "PerformanceReview", new { Id = input.Id });
        }

        [Abp.Authorization.AbpAuthorize(PermissionNames.PerformanceReview_Update)]
        public async Task<PerformanceReviewDto> ChangeStatusAsync(long id, ChangeStatusInput input)
        {
            var entity = await Repository.GetAsync(id);
            var currentStatus = entity.Status.ToString();

            // Find valid transition
            var transitions = new (string From, string To, string Action, bool Readonly)[]
            {
            ("Draft", "SelfAssessmentPending", "Start", false),
            ("SelfAssessmentPending", "ManagerReviewPending", "Approve", false),
            ("ManagerReviewPending", "PeerReviewPending", "Approve", false),
            ("ManagerReviewPending", "SelfAssessmentPending", "Revise", false),
            ("PeerReviewPending", "HRReviewPending", "Approve", false),
            ("HRReviewPending", "Completed", "Approve", true),
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
            entity.Status = (PerformanceReviewStatus)Enum.Parse(typeof(PerformanceReviewStatus), transition.To);
            await Repository.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            // Cancel pending ApprovalRecords when the entity is cancelled — otherwise the records
            // sit forever in approvers' inboxes pointing to a cancelled request.
            if (input.Action == "Cancel")
            {
                var pending = _approvalRepo.GetAll()
                    .Where(a => a.EntityType == "PerformanceReview" && a.EntityId == id.ToString() && a.Status == "Pending")
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
                EntityType = "PerformanceReview",
                EntityId = id.ToString(),
                FromStatus = fromStatus,
                ToStatus = transition.To,
                Action = input.Action,
                Comment = input.ActionData != null && input.ActionData.ContainsKey("comment") ? input.ActionData["comment"] : null,
                ChangedByUserId = AbpSession.UserId
            });

            var result = MapToEntityDto(entity);

            // Trigger flow: on-status-change (always)
            await _flowEngine.TriggerAsync("on-field-change", "PerformanceReview", result);

            // Trigger named flow events
            if (input.Action == "Start")
                await _flowEngine.TriggerAsync("submit-for-approval", "PerformanceReview", result);
            return result;
        }

        private void ValidateStatusTransition(PerformanceReviewStatus from, PerformanceReviewStatus to)
        {
            var allowed = new (string From, string To)[]
            {
                ("Draft", "SelfAssessmentPending"),
                ("SelfAssessmentPending", "ManagerReviewPending"),
                ("ManagerReviewPending", "PeerReviewPending"),
                ("ManagerReviewPending", "SelfAssessmentPending"),
                ("PeerReviewPending", "HRReviewPending"),
                ("HRReviewPending", "Completed"),
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
