using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.PurchaseOrders.Dto
{
    public class PagedPurchaseOrderResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? PurchaseRequestId { get; set; }
        public long? SupplierId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public decimal? OrderAmount { get; set; }
        public string Notes { get; set; }
        public string RejectionReason { get; set; }
        public int? Status { get; set; }
        public DateTime? OrderDateFrom { get; set; }
        public DateTime? OrderDateTo { get; set; }
        public DateTime? DeliveryDateFrom { get; set; }
        public DateTime? DeliveryDateTo { get; set; }
        public decimal? OrderAmountFrom { get; set; }
        public decimal? OrderAmountTo { get; set; }
        /// <summary>Virgülle ayrılmış enum indeksleri — ör. "0,2"</summary>
        public string StatusIn { get; set; }
        public int? StatusNot { get; set; }
    }
}
