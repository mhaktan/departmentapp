using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.SalaryRecords.Dto
{
    [AutoMapFrom(typeof(Entities.SalaryRecord))]
    public class SalaryRecordDto : EntityDto<long>
    {
        public DateTime EffectiveDate { get; set; }

        public decimal GrossSalary { get; set; }

        public decimal? NetSalary { get; set; }

        public string Currency { get; set; }

        public int SalaryType { get; set; }

        public string Notes { get; set; }

        public long EmployeeId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}