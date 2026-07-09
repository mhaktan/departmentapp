using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    // State Machine: status — Open → UnderReview → Appealed → Resolved → Closed
    // Initial: Open | Transitions: Open→UnderReview[StartReview], UnderReview→Resolved[Resolve], UnderReview→Appealed[Appeal], Appealed→UnderReview[Reopen], Resolved→Closed[Close], Appealed→Closed[Close]
    [Table("DisciplinaryRecords")]
    public class DisciplinaryRecord : FullAuditedEntity<long>
    {
        public DateTime IncidentDate { get; set; }

        public DisciplinaryRecordType Type { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        [MaxLength(1000)]
        public string ActionTaken { get; set; }

        [MaxLength(200)]
        public string IssuedBy { get; set; }

        public bool AcknowledgedByEmployee { get; set; }

        public DisciplinaryRecordStatus Status { get; set; }

        [MaxLength(1000)]
        public string AppealNote { get; set; }

        [MaxLength(1000)]
        public string ResolutionNote { get; set; }

        public long? HrReviewerId { get; set; }

        public long? HrManagerResolverId { get; set; }

        public long EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

    }
}