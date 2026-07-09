using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.Trainings.Dto;

namespace DepartmentApp.Trainings
{
    public interface ITrainingAppService : IAsyncCrudAppService<
        TrainingDto,
        long,
        PagedTrainingResultRequestDto,
        CreateTrainingDto,
        TrainingDto>
    {
        Task<TrainingDto> ChangeStatusAsync(long id, ChangeStatusInput input);
    }
}
