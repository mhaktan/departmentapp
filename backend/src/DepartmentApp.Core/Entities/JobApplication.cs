using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    // State Machine: status — Received → Screening → Interview → OfferPending → OfferAccepted → OfferRejected → Rejected
    // Initial: Received | Transitions: Received→Screening[StartScreening], Screening→Interview[InviteToInterview], Screening→Rejected[Reject], Interview→OfferPending[MakeOffer], Interview→Rejected[Reject], OfferPending→OfferAccepted[AcceptOffer], OfferPending→OfferRejected[RejectOffer]
    [Table("JobApplications")]
    public class JobApplication : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(100)]
        public string ApplicantFirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string ApplicantLastName { get; set; }

        [Required]
        [MaxLength(256)]
        public string ApplicantEmail { get; set; }

        [MaxLength(50)]
        public string ApplicantPhone { get; set; }

        [MaxLength(3000)]
        public string CoverLetter { get; set; }

        public Status Status { get; set; }

        [MaxLength(1000)]
        public string ScreeningNotes { get; set; }

        public DateTime? InterviewDate { get; set; }

        [MaxLength(2000)]
        public string InterviewNotes { get; set; }

        public decimal? OfferSalary { get; set; }

        public DateTime? OfferDate { get; set; }

        [MaxLength(500)]
        public string RejectionReason { get; set; }

        public long JobPostingId { get; set; }

        [ForeignKey(nameof(JobPostingId))]
        public virtual JobPosting JobPosting { get; set; }

        public virtual ICollection<Onboarding> Onboardings { get; set; }

    }
}