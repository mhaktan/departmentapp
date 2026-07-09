using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.SalaryDeductions.Dto
{
    [AutoMapTo(typeof(Entities.SalaryDeduction))]
    public class CreateSalaryDeductionDto
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

    }
}