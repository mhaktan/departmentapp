using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.SalaryRecords.Dto
{
    public class PagedSalaryRecordResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? EmployeeId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? GrossSalary { get; set; }
        public decimal? NetSalary { get; set; }
        public string Currency { get; set; }
        public int? SalaryType { get; set; }
        public string Notes { get; set; }
    }
}
