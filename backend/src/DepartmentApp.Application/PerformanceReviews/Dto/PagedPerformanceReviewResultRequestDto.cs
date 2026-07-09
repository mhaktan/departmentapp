using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.PerformanceReviews.Dto
{
    public class PagedPerformanceReviewResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? EmployeeId { get; set; }
        public string ReviewPeriod { get; set; }
        public int? ReviewYear { get; set; }
        public int? ReviewType { get; set; }
        public int? Status { get; set; }
        public decimal? SelfAssessmentScore { get; set; }
        public string SelfAssessmentNotes { get; set; }
        public decimal? ManagerScore { get; set; }
        public string ManagerNotes { get; set; }
        public decimal? OverallScore { get; set; }
        public string HrNotes { get; set; }
        public string RevisionNote { get; set; }
        public long? ManagerReviewerId { get; set; }
        public long? HrReviewerId { get; set; }
        public long? PeerReviewersAssignedBy { get; set; }
    }
}
