using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    // State Machine: status — Draft → Active → Completed → Cancelled
    // Initial: Draft | Transitions: Draft→Active[Activate], Active→Completed[Complete], *→Cancelled[Cancel]
    [Table("TrainingPlans")]
    public class TrainingPlan : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public int Year { get; set; }

        public Status Status { get; set; }

        public long DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        public virtual ICollection<Training> Trainings { get; set; }

    }
}