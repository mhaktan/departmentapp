using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.TrainingPlans.Dto
{
    [AutoMapFrom(typeof(Entities.TrainingPlan))]
    public class TrainingPlanDto : EntityDto<long>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }

        public int Status { get; set; }

        /// <summary>
        /// String form of the status — used by flow conditions (triggerData.statusName equals "PendingX").
        /// </summary>
        public string StatusName { get; set; }

        public long DepartmentId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}