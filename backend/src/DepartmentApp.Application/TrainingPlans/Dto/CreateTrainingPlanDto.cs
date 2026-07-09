using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.TrainingPlans.Dto
{
    [AutoMapTo(typeof(Entities.TrainingPlan))]
    public class CreateTrainingPlanDto
    {
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public int Year { get; set; }

        public int Status { get; set; }

        public long DepartmentId { get; set; }

    }
}