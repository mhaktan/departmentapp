using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.TrainingParticipations.Dto;

namespace DepartmentApp.TrainingParticipations
{
    public interface ITrainingParticipationAppService : IAsyncCrudAppService<
        TrainingParticipationDto,
        long,
        PagedTrainingParticipationResultRequestDto,
        CreateTrainingParticipationDto,
        TrainingParticipationDto>
    {
    }
}
