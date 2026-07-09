using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.Trainings.Dto
{
    public class PagedTrainingResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? TrainingPlanId { get; set; }
        public string Title { get; set; }
        public string Provider { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Location { get; set; }
        public int? TrainingType { get; set; }
        public int? Status { get; set; }
        public decimal? Cost { get; set; }
        public string Currency { get; set; }
    }
}
