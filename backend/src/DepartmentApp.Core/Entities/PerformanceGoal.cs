using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    [Table("PerformanceGoals")]
    public class PerformanceGoal : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public DateTime? TargetDate { get; set; }

        public decimal? Weight { get; set; }

        public decimal? SelfScore { get; set; }

        public decimal? ManagerScore { get; set; }

        public Status Status { get; set; }

        public long PerformanceReviewId { get; set; }

        [ForeignKey(nameof(PerformanceReviewId))]
        public virtual PerformanceReview PerformanceReview { get; set; }

    }
}