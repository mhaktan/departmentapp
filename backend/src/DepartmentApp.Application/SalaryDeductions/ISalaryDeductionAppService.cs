using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.SalaryDeductions.Dto;

namespace DepartmentApp.SalaryDeductions
{
    public interface ISalaryDeductionAppService : IAsyncCrudAppService<
        SalaryDeductionDto,
        long,
        PagedSalaryDeductionResultRequestDto,
        CreateSalaryDeductionDto,
        SalaryDeductionDto>
    {
    }
}
