using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.PeerReviews.Dto
{
    public class PagedPeerReviewResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? PerformanceReviewId { get; set; }
        public long? EmployeeId { get; set; }
        public string ReviewerName { get; set; }
        public decimal? Score { get; set; }
        public string Strengths { get; set; }
        public string Improvements { get; set; }
        public bool? IsAnonymous { get; set; }
    }
}
