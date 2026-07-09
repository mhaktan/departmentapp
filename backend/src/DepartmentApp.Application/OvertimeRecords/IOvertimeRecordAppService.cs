using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.OvertimeRecords.Dto;

namespace DepartmentApp.OvertimeRecords
{
    public interface IOvertimeRecordAppService : IAsyncCrudAppService<
        OvertimeRecordDto,
        long,
        PagedOvertimeRecordResultRequestDto,
        CreateOvertimeRecordDto,
        OvertimeRecordDto>
    {
        Task<OvertimeRecordDto> ChangeStatusAsync(long id, ChangeStatusInput input);
    }
}
