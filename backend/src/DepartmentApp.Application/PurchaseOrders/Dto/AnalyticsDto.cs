using System;
using DepartmentApp.Analytics.Dto;

namespace DepartmentApp.PurchaseOrders.Dto
{
    /// <summary>GetAll ile ayni filtreleri kabul eder, ustune GroupBy alir.</summary>
    public class PurchaseOrderGroupedCountInput : PagedPurchaseOrderResultRequestDto
    {
        public string GroupBy { get; set; }
    }

    public class PurchaseOrderStatsInput : PagedPurchaseOrderResultRequestDto
    {
        /// <summary>avg | sum | min | max | avgDayDiff</summary>
        public string Aggregate { get; set; }
        public string Field { get; set; }
        public string FromField { get; set; }
        public string ToField { get; set; }
    }
}
