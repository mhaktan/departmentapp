using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.PerformanceReviews.Dto;

namespace DepartmentApp.PerformanceReviews
{
    public interface IPerformanceReviewAppService : IAsyncCrudAppService<
        PerformanceReviewDto,
        long,
        PagedPerformanceReviewResultRequestDto,
        CreatePerformanceReviewDto,
        PerformanceReviewDto>
    {
        Task<PerformanceReviewDto> ChangeStatusAsync(long id, ChangeStatusInput input);
    }
}
