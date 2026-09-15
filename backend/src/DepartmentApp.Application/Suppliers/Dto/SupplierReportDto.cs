using System;
using System.Collections.Generic;
using DepartmentApp.Quotations.Dto;
using DepartmentApp.PurchaseOrders.Dto;

namespace DepartmentApp.Suppliers.Dto
{
    /// <summary>
    /// Rapor verisi — kok kayit ve alt koleksiyonlar tek yanitta.
    /// PDF sablonu tek apiBinding kullandigi icin nested donuyoruz.
    /// </summary>
    public class SupplierReportDto
    {
        public SupplierDto Data { get; set; }
        public List<QuotationDto> Quotations { get; set; }
        public List<PurchaseOrderDto> PurchaseOrders { get; set; }
    }
}
