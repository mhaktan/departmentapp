using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.DisciplinaryRecords.Dto;

namespace DepartmentApp.DisciplinaryRecords
{
    public interface IDisciplinaryRecordAppService : IAsyncCrudAppService<
        DisciplinaryRecordDto,
        long,
        PagedDisciplinaryRecordResultRequestDto,
        CreateDisciplinaryRecordDto,
        DisciplinaryRecordDto>
    {
        Task<DisciplinaryRecordDto> ChangeStatusAsync(long id, ChangeStatusInput input);
    }
}
