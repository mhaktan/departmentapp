using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.SalaryDeductions.Dto
{
    [AutoMapFrom(typeof(Entities.SalaryDeduction))]
    public class SalaryDeductionDto : EntityDto<long>
    {
        public string DeductionType { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string Description { get; set; }

        public long SalaryRecordId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}