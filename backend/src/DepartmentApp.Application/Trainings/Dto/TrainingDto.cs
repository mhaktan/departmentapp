using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.Trainings.Dto
{
    [AutoMapFrom(typeof(Entities.Training))]
    public class TrainingDto : EntityDto<long>
    {
        public string Title { get; set; }

        public string Provider { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Location { get; set; }

        public int TrainingType { get; set; }

        public int Status { get; set; }

        public decimal? Cost { get; set; }

        public string Currency { get; set; }

        /// <summary>
        /// String form of the status — used by flow conditions (triggerData.statusName equals "PendingX").
        /// </summary>
        public string StatusName { get; set; }

        public long TrainingPlanId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}