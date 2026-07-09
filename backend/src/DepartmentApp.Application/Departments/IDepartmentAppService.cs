using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.Departments.Dto;

namespace DepartmentApp.Departments
{
    public interface IDepartmentAppService : IAsyncCrudAppService<
        DepartmentDto,
        long,
        PagedDepartmentResultRequestDto,
        CreateDepartmentDto,
        DepartmentDto>
    {
    }
}
