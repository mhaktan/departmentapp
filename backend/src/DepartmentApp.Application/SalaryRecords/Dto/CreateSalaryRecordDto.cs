using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.SalaryRecords.Dto
{
    [AutoMapTo(typeof(Entities.SalaryRecord))]
    public class CreateSalaryRecordDto
    {
        public DateTime EffectiveDate { get; set; }

        public decimal GrossSalary { get; set; }

        public decimal? NetSalary { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; }

        public int SalaryType { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public long EmployeeId { get; set; }

    }
}