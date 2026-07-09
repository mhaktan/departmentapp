using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    [Table("PeerReviews")]
    public class PeerReview : FullAuditedEntity<long>
    {
        [MaxLength(200)]
        public string ReviewerName { get; set; }

        public decimal? Score { get; set; }

        [MaxLength(2000)]
        public string Strengths { get; set; }

        [MaxLength(2000)]
        public string Improvements { get; set; }

        public bool IsAnonymous { get; set; }

        public long PerformanceReviewId { get; set; }

        [ForeignKey(nameof(PerformanceReviewId))]
        public virtual PerformanceReview PerformanceReview { get; set; }

        public long EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

    }
}