using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.PerformanceGoals.Dto
{
    public class PagedPerformanceGoalResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? PerformanceReviewId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? TargetDate { get; set; }
        public decimal? Weight { get; set; }
        public decimal? SelfScore { get; set; }
        public decimal? ManagerScore { get; set; }
        public int? Status { get; set; }
    }
}
