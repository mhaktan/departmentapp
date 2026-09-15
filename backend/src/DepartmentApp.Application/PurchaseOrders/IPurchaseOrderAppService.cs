using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.Analytics.Dto;
using DepartmentApp.StateMachine.Dto;
using DepartmentApp.PurchaseOrders.Dto;

namespace DepartmentApp.PurchaseOrders
{
    public interface IPurchaseOrderAppService : IAsyncCrudAppService<
        PurchaseOrderDto,
        long,
        PagedPurchaseOrderResultRequestDto,
        CreatePurchaseOrderDto,
        PurchaseOrderDto>
    {
        Task<PurchaseOrderDto> ChangeStatusAsync(long id, ChangeStatusInput input);
        List<GroupCountDto> GetGroupedCount(PurchaseOrderGroupedCountInput input);
        decimal? GetStats(PurchaseOrderStatsInput input);
    }
}
