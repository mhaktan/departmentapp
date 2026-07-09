using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.PerformanceGoals.Dto
{
    [AutoMapFrom(typeof(Entities.PerformanceGoal))]
    public class PerformanceGoalDto : EntityDto<long>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? TargetDate { get; set; }

        public decimal? Weight { get; set; }

        public decimal? SelfScore { get; set; }

        public decimal? ManagerScore { get; set; }

        public int Status { get; set; }

        public long PerformanceReviewId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}