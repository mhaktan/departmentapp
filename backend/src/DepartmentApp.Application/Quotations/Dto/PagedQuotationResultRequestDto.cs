using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.Quotations.Dto
{
    public class PagedQuotationResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? PurchaseRequestId { get; set; }
        public long? SupplierId { get; set; }
        public DateTime? QuotationDate { get; set; }
        public decimal? Amount { get; set; }
        public bool? IsSelected { get; set; }
        public DateTime? QuotationDateFrom { get; set; }
        public DateTime? QuotationDateTo { get; set; }
        public decimal? AmountFrom { get; set; }
        public decimal? AmountTo { get; set; }
    }
}
