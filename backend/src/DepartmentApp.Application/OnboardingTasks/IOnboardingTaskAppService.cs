using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.OnboardingTasks.Dto;

namespace DepartmentApp.OnboardingTasks
{
    public interface IOnboardingTaskAppService : IAsyncCrudAppService<
        OnboardingTaskDto,
        long,
        PagedOnboardingTaskResultRequestDto,
        CreateOnboardingTaskDto,
        OnboardingTaskDto>
    {
    }
}
