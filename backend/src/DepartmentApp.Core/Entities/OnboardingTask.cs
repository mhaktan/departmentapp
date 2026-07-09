using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    [Table("OnboardingTasks")]
    public class OnboardingTask : FullAuditedEntity<long>
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

        [ForeignKey(nameof(OnboardingId))]
        public virtual Onboarding Onboarding { get; set; }

    }
}