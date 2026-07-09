using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.Onboardings.Dto;

namespace DepartmentApp.Onboardings
{
    public interface IOnboardingAppService : IAsyncCrudAppService<
        OnboardingDto,
        long,
        PagedOnboardingResultRequestDto,
        CreateOnboardingDto,
        OnboardingDto>
    {
        Task<OnboardingDto> ChangeStatusAsync(long id, ChangeStatusInput input);
    }
}
