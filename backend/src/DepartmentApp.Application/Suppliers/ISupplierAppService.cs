using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.Analytics.Dto;
using DepartmentApp.Suppliers.Dto;

namespace DepartmentApp.Suppliers
{
    public interface ISupplierAppService : IAsyncCrudAppService<
        SupplierDto,
        long,
        PagedSupplierResultRequestDto,
        CreateSupplierDto,
        SupplierDto>
    {
        Task<SupplierReportDto> GetReportData(long id);
    }
}
