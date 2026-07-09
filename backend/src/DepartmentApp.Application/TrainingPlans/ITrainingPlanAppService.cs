using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.TrainingPlans.Dto;

namespace DepartmentApp.TrainingPlans
{
    public interface ITrainingPlanAppService : IAsyncCrudAppService<
        TrainingPlanDto,
        long,
        PagedTrainingPlanResultRequestDto,
        CreateTrainingPlanDto,
        TrainingPlanDto>
    {
        Task<TrainingPlanDto> ChangeStatusAsync(long id, ChangeStatusInput input);
    }
}
