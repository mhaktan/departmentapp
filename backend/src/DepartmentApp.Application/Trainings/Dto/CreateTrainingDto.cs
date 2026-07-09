using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.Trainings.Dto
{
    [AutoMapTo(typeof(Entities.Training))]
    public class CreateTrainingDto
    {
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Provider { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(300)]
        public string Location { get; set; }

        public int TrainingType { get; set; }

        public int Status { get; set; }

        public decimal? Cost { get; set; }

        [MaxLength(10)]
        public string Currency { get; set; }

        public long TrainingPlanId { get; set; }

    }
}