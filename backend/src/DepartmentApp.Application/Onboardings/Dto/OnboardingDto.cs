using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.Onboardings.Dto
{
    [AutoMapFrom(typeof(Entities.Onboarding))]
    public class OnboardingDto : EntityDto<long>
    {
        public DateTime StartDate { get; set; }

        public DateTime? ExpectedCompletionDate { get; set; }

        public int Status { get; set; }

        public string Notes { get; set; }

        /// <summary>
        /// String form of the status — used by flow conditions (triggerData.statusName equals "PendingX").
        /// </summary>
        public string StatusName { get; set; }

        public long JobApplicationId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}