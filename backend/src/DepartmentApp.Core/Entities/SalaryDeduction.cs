using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    [Table("SalaryDeductions")]
    public class SalaryDeduction : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(200)]
        public string DeductionType { get; set; }

        public decimal Amount { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; }

        public DateTime EffectiveDate { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public long SalaryRecordId { get; set; }

        [ForeignKey(nameof(SalaryRecordId))]
        public virtual SalaryRecord SalaryRecord { get; set; }

    }
}