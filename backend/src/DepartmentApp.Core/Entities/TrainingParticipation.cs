using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    [Table("TrainingParticipations")]
    public class TrainingParticipation : FullAuditedEntity<long>
    {
        public bool Attended { get; set; }

        public DateTime? CompletionDate { get; set; }

        public decimal? Score { get; set; }

        public bool? CertificateEarned { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public long TrainingId { get; set; }

        [ForeignKey(nameof(TrainingId))]
        public virtual Training Training { get; set; }

        public long EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

    }
}