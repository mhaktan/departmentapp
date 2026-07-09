using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.PerformanceGoals.Dto
{
    [AutoMapTo(typeof(Entities.PerformanceGoal))]
    public class CreatePerformanceGoalDto
    {
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public DateTime? TargetDate { get; set; }

        public decimal? Weight { get; set; }

        public decimal? SelfScore { get; set; }

        public decimal? ManagerScore { get; set; }

        public int Status { get; set; }

        public long PerformanceReviewId { get; set; }

    }
}