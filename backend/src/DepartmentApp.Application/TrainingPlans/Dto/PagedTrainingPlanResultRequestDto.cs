using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.TrainingPlans.Dto
{
    public class PagedTrainingPlanResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? DepartmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Year { get; set; }
        public int? Status { get; set; }
    }
}
