using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.Analytics.Dto;
using DepartmentApp.PurchaseRequestItems.Dto;

namespace DepartmentApp.PurchaseRequestItems
{
    public interface IPurchaseRequestItemAppService : IAsyncCrudAppService<
        PurchaseRequestItemDto,
        long,
        PagedPurchaseRequestItemResultRequestDto,
        CreatePurchaseRequestItemDto,
        PurchaseRequestItemDto>
    {
        List<GroupCountDto> GetGroupedCount(PurchaseRequestItemGroupedCountInput input);
        decimal? GetStats(PurchaseRequestItemStatsInput input);
    }
}
