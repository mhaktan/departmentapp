using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    // State Machine: status — Draft → Published → Closed → Cancelled
    // Initial: Draft | Transitions: Draft→Published[Publish], Published→Closed[Close], *→Cancelled[Cancel]
    [Table("JobPostings")]
    public class JobPosting : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [MaxLength(5000)]
        public string Description { get; set; }

        [MaxLength(3000)]
        public string Requirements { get; set; }

        public int PositionCount { get; set; }

        public Status Status { get; set; }

        public DateTime? PublishDate { get; set; }

        public DateTime? ClosingDate { get; set; }

        public EmploymentType EmploymentType { get; set; }

        public long DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        public virtual ICollection<JobApplication> JobApplications { get; set; }

    }
}