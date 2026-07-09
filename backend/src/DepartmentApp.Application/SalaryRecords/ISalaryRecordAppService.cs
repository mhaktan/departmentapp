using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.SalaryRecords.Dto;

namespace DepartmentApp.SalaryRecords
{
    public interface ISalaryRecordAppService : IAsyncCrudAppService<
        SalaryRecordDto,
        long,
        PagedSalaryRecordResultRequestDto,
        CreateSalaryRecordDto,
        SalaryRecordDto>
    {
    }
}
