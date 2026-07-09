using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.OnboardingTasks.Dto
{
    [AutoMapTo(typeof(Entities.OnboardingTask))]
    public class CreateOnboardingTaskDto
    {
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedDate { get; set; }

        [MaxLength(200)]
        public string AssignedTo { get; set; }

        public DateTime? DueDate { get; set; }

        public long OnboardingId { get; set; }

    }
}