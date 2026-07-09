using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.OnboardingTasks.Dto
{
    public class PagedOnboardingTaskResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? OnboardingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string AssignedTo { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
