using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.JobApplications.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.JobApplications
{
    public class JobApplicationAppService : AsyncCrudAppService<
        JobApplication,
        JobApplicationDto,
        long,
        PagedJobApplicationResultRequestDto,
        CreateJobApplicationDto,
        JobApplicationDto>,
        IJobApplicationAppService
    {
        private readonly IRepository<StatusChangeLog, long> _statusChangeLogRepo;
        private readonly IRepository<ApprovalRecord, Guid> _approvalRepo;
        private readonly IFlowEngine _flowEngine;

        public JobApplicationAppService(IRepository<JobApplication, long> repository, IFlowEngine flowEngine, IRepository<StatusChangeLog, long> statusChangeLogRepo, IRepository<ApprovalRecord, Guid> approvalRepo)
            : base(repository)
        {
            _flowEngine = flowEngine;
            _statusChangeLogRepo = statusChangeLogRepo;
            _approvalRepo = approvalRepo;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.JobApplication_Read;
            GetAllPermissionName = PermissionNames.JobApplication_Read;
            CreatePermissionName = PermissionNames.JobApplication_Create;
            UpdatePermissionName = PermissionNames.JobApplication_Update;
            DeletePermissionName = PermissionNames.JobApplication_Delete;
        }

        protected override IQueryable<JobApplication> CreateFilteredQuery(PagedJobApplicationResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.ApplicantFirstName != null && x.ApplicantFirstName.Contains(input.Keyword)) ||
                    (x.ApplicantLastName != null && x.ApplicantLastName.Contains(input.Keyword)) ||
                    (x.ApplicantEmail != null && x.ApplicantEmail.Contains(input.Keyword)) ||
                    (x.ApplicantPhone != null && x.ApplicantPhone.Contains(input.Keyword)) ||
                    (x.CoverLetter != null && x.CoverLetter.Contains(input.Keyword)) ||
                    (x.ScreeningNotes != null && x.ScreeningNotes.Contains(input.Keyword)) ||
                    (x.InterviewNotes != null && x.InterviewNotes.Contains(input.Keyword)) ||
                    (x.RejectionReason != null && x.RejectionReason.Contains(input.Keyword)))
                .WhereIf(!input.ApplicantFirstName.IsNullOrWhiteSpace(), x => x.ApplicantFirstName != null && x.ApplicantFirstName.Contains(input.ApplicantFirstName))
                .WhereIf(!input.ApplicantLastName.IsNullOrWhiteSpace(), x => x.ApplicantLastName != null && x.ApplicantLastName.Contains(input.ApplicantLastName))
                .WhereIf(!input.ApplicantEmail.IsNullOrWhiteSpace(), x => x.ApplicantEmail != null && x.ApplicantEmail.Contains(input.ApplicantEmail))
                .WhereIf(!input.ApplicantPhone.IsNullOrWhiteSpace(), x => x.ApplicantPhone != null && x.ApplicantPhone.Contains(input.ApplicantPhone))
                .WhereIf(!input.CoverLetter.IsNullOrWhiteSpace(), x => x.CoverLetter != null && x.CoverLetter.Contains(input.CoverLetter))
                .WhereIf(!input.ScreeningNotes.IsNullOrWhiteSpace(), x => x.ScreeningNotes != null && x.ScreeningNotes.Contains(input.ScreeningNotes))
                .WhereIf(!input.InterviewNotes.IsNullOrWhiteSpace(), x => x.InterviewNotes != null && x.InterviewNotes.Contains(input.InterviewNotes))
                .WhereIf(!input.RejectionReason.IsNullOrWhiteSpace(), x => x.RejectionReason != null && x.RejectionReason.Contains(input.RejectionReason))
                .WhereIf(input.Status.HasValue, x => x.Status == (Status)input.Status.Value)
                .WhereIf(input.InterviewDate.HasValue, x => x.InterviewDate == input.InterviewDate.Value)
                .WhereIf(input.OfferSalary.HasValue, x => x.OfferSalary == input.OfferSalary.Value)
                .WhereIf(input.OfferDate.HasValue, x => x.OfferDate == input.OfferDate.Value)
                .WhereIf(input.JobPostingId.HasValue, x => x.JobPostingId == input.JobPostingId.Value);
        }

        public override async Task<JobApplicationDto> CreateAsync(CreateJobApplicationDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "JobApplication", result);

            // Frontend creates records with status pre-set without going through ChangeStatusAsync,
            // so mirror on-field-change here whenever the initial status isn't the default. Otherwise
            // status-driven flows (e.g. approval) never fire on plain Create.
            if (result.Status != (int)Status.Received)
                await _flowEngine.TriggerAsync("on-field-change", "JobApplication", result);
            return result;
        }

        public override async Task<JobApplicationDto> UpdateAsync(JobApplicationDto input)
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
                    EntityType = "JobApplication",
                    EntityId = input.Id.ToString(),
                    FromStatus = fromStatus,
                    ToStatus = toStatus,
                    Action = "Update",
                    ChangedByUserId = AbpSession.UserId
                });
            }

            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "JobApplication", result);

            // Frontend updates status via plain UpdateAsync (not ChangeStatusAsync) — fire
            // on-field-change so status-driven flows pick up the transition.
            if (statusChanged)
                await _flowEngine.TriggerAsync("on-field-change", "JobApplication", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "JobApplication", new { Id = input.Id });
        }

        [Abp.Authorization.AbpAuthorize(PermissionNames.JobApplication_Update)]
        public async Task<JobApplicationDto> ChangeStatusAsync(long id, ChangeStatusInput input)
        {
            var entity = await Repository.GetAsync(id);
            var currentStatus = entity.Status.ToString();

            // Find valid transition
            var transitions = new (string From, string To, string Action, bool Readonly)[]
            {
            ("Received", "Screening", "StartScreening", false),
            ("Screening", "Interview", "InviteToInterview", false),
            ("Screening", "Rejected", "Reject", true),
            ("Interview", "OfferPending", "MakeOffer", false),
            ("Interview", "Rejected", "Reject", true),
            ("OfferPending", "OfferAccepted", "AcceptOffer", true),
            ("OfferPending", "OfferRejected", "RejectOffer", true)
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
                    .Where(a => a.EntityType == "JobApplication" && a.EntityId == id.ToString() && a.Status == "Pending")
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
                EntityType = "JobApplication",
                EntityId = id.ToString(),
                FromStatus = fromStatus,
                ToStatus = transition.To,
                Action = input.Action,
                Comment = input.ActionData != null && input.ActionData.ContainsKey("comment") ? input.ActionData["comment"] : null,
                ChangedByUserId = AbpSession.UserId
            });

            var result = MapToEntityDto(entity);

            // Trigger flow: on-status-change (always)
            await _flowEngine.TriggerAsync("on-field-change", "JobApplication", result);

            return result;
        }

        private void ValidateStatusTransition(Status from, Status to)
        {
            var allowed = new (string From, string To)[]
            {
                ("Received", "Screening"),
                ("Screening", "Interview"),
                ("Screening", "Rejected"),
                ("Interview", "OfferPending"),
                ("Interview", "Rejected"),
                ("OfferPending", "OfferAccepted"),
                ("OfferPending", "OfferRejected")
            };

            var isValid = allowed.Any(t =>
                (t.From == "*" || t.From == from.ToString()) &&
                t.To == to.ToString());

            if (!isValid)
                throw new Abp.UI.UserFriendlyException($"Invalid status transition from {from} to {to}");
        }
    }
}
