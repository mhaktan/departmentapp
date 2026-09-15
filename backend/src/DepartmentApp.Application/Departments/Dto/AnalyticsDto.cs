using System;
using DepartmentApp.Analytics.Dto;

namespace DepartmentApp.Departments.Dto
{
    /// <summary>GetAll ile ayni filtreleri kabul eder, ustune GroupBy alir.</summary>
    public class DepartmentGroupedCountInput : PagedDepartmentResultRequestDto
    {
        public string GroupBy { get; set; }
    }

    public class DepartmentStatsInput : PagedDepartmentResultRequestDto
    {
        /// <summary>avg | sum | min | max | avgDayDiff</summary>
        public string Aggregate { get; set; }
        public string Field { get; set; }
        public string FromField { get; set; }
        public string ToField { get; set; }
    }
}
