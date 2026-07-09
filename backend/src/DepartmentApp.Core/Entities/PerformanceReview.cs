using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    // State Machine: status — Draft → SelfAssessmentPending → ManagerReviewPending → PeerReviewPending → HRReviewPending → Completed → Cancelled
    // Initial: Draft | Transitions: Draft→SelfAssessmentPending[Start], SelfAssessmentPending→ManagerReviewPending[Approve], ManagerReviewPending→PeerReviewPending[Approve], ManagerReviewPending→SelfAssessmentPending[Revise], PeerReviewPending→HRReviewPending[Approve], HRReviewPending→Completed[Approve], *→Cancelled[Cancel]
    [Table("PerformanceReviews")]
    public class PerformanceReview : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(100)]
        public string ReviewPeriod { get; set; }

        public int ReviewYear { get; set; }

        public ReviewType ReviewType { get; set; }

        public Status Status { get; set; }

        public decimal? SelfAssessmentScore { get; set; }

        [MaxLength(2000)]
        public string SelfAssessmentNotes { get; set; }

        public decimal? ManagerScore { get; set; }

        [MaxLength(2000)]
        public string ManagerNotes { get; set; }

        public decimal? OverallScore { get; set; }

        [MaxLength(2000)]
        public string HrNotes { get; set; }

        [MaxLength(1000)]
        public string RevisionNote { get; set; }

        public long? ManagerReviewerId { get; set; }

        public long? HrReviewerId { get; set; }

        public long? PeerReviewersAssignedBy { get; set; }

        public long EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        public virtual ICollection<PerformanceGoal> PerformanceGoals { get; set; }

        public virtual ICollection<PeerReview> PeerReviews { get; set; }

    }
}