using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    [Table("SalaryRecords")]
    public class SalaryRecord : FullAuditedEntity<long>
    {
        public DateTime EffectiveDate { get; set; }

        public decimal GrossSalary { get; set; }

        public decimal? NetSalary { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; }

        public SalaryRecordSalaryType SalaryType { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public long EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        public virtual ICollection<SalaryDeduction> SalaryDeductions { get; set; }

    }
}