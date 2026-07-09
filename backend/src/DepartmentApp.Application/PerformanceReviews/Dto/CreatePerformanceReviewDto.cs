using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.PerformanceReviews.Dto
{
    [AutoMapTo(typeof(Entities.PerformanceReview))]
    public class CreatePerformanceReviewDto
    {
        [Required]
        [MaxLength(100)]
        public string ReviewPeriod { get; set; }

        public int ReviewYear { get; set; }

        public int ReviewType { get; set; }

        public int Status { get; set; }

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

    }
}