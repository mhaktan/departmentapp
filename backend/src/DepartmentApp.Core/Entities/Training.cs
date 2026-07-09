using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    // State Machine: status — Planned → Ongoing → Completed → Cancelled
    // Initial: Planned | Transitions: Planned→Ongoing[Start], Ongoing→Completed[Complete], *→Cancelled[Cancel]
    [Table("Trainings")]
    public class Training : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Provider { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(300)]
        public string Location { get; set; }

        public TrainingType TrainingType { get; set; }

        public Status Status { get; set; }

        public decimal? Cost { get; set; }

        [MaxLength(10)]
        public string Currency { get; set; }

        public long TrainingPlanId { get; set; }

        [ForeignKey(nameof(TrainingPlanId))]
        public virtual TrainingPlan TrainingPlan { get; set; }

        public virtual ICollection<TrainingParticipation> TrainingParticipations { get; set; }

    }
}