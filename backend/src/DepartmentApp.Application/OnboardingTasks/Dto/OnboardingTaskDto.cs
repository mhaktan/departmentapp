using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.OnboardingTasks.Dto
{
    [AutoMapFrom(typeof(Entities.OnboardingTask))]
    public class OnboardingTaskDto : EntityDto<long>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedDate { get; set; }

        public string AssignedTo { get; set; }

        public DateTime? DueDate { get; set; }

        public long OnboardingId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}