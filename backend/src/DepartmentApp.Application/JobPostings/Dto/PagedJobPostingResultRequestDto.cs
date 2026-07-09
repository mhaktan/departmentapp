using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.JobPostings.Dto
{
    public class PagedJobPostingResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? DepartmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public int? PositionCount { get; set; }
        public int? Status { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public int? EmploymentType { get; set; }
    }
}
