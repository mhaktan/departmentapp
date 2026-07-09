using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.Onboardings.Dto
{
    public class PagedOnboardingResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? JobApplicationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpectedCompletionDate { get; set; }
        public int? Status { get; set; }
        public string Notes { get; set; }
    }
}
