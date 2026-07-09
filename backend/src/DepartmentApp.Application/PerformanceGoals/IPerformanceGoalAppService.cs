using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.PerformanceGoals.Dto;

namespace DepartmentApp.PerformanceGoals
{
    public interface IPerformanceGoalAppService : IAsyncCrudAppService<
        PerformanceGoalDto,
        long,
        PagedPerformanceGoalResultRequestDto,
        CreatePerformanceGoalDto,
        PerformanceGoalDto>
    {
    }
}
