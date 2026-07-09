using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    // State Machine: status — NotStarted → InProgress → Completed → Cancelled
    // Initial: NotStarted | Transitions: NotStarted→InProgress[Start], InProgress→Completed[Complete], *→Cancelled[Cancel]
    [Table("Onboardings")]
    public class Onboarding : FullAuditedEntity<long>
    {
        public DateTime StartDate { get; set; }

        public DateTime? ExpectedCompletionDate { get; set; }

        public Status Status { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

        public long JobApplicationId { get; set; }

        [ForeignKey(nameof(JobApplicationId))]
        public virtual JobApplication JobApplication { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<OnboardingTask> OnboardingTasks { get; set; }

    }
}