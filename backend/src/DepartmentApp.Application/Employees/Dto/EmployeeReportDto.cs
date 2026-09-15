using System;
using System.Collections.Generic;
using DepartmentApp.PurchaseRequests.Dto;

namespace DepartmentApp.Employees.Dto
{
    /// <summary>
    /// Rapor verisi — kok kayit ve alt koleksiyonlar tek yanitta.
    /// PDF sablonu tek apiBinding kullandigi icin nested donuyoruz.
    /// </summary>
    public class EmployeeReportDto
    {
        public EmployeeDto Data { get; set; }
        public List<PurchaseRequestDto> PurchaseRequests { get; set; }
    }
}
