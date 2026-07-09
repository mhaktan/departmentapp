using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.SalaryDeductions.Dto
{
    public class PagedSalaryDeductionResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? SalaryRecordId { get; set; }
        public string DeductionType { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string Description { get; set; }
    }
}
