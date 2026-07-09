using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    [Table("EmployeeCertificates")]
    public class EmployeeCertificate : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(200)]
        public string CertificateName { get; set; }

        [MaxLength(200)]
        public string IssuingBody { get; set; }

        public DateTime? IssueDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [MaxLength(100)]
        public string CertificateNumber { get; set; }

        public long EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

    }
}