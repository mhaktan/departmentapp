using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.LeaveTypes.Dto;

namespace DepartmentApp.LeaveTypes
{
    public interface ILeaveTypeAppService : IAsyncCrudAppService<
        LeaveTypeDto,
        long,
        PagedLeaveTypeResultRequestDto,
        CreateLeaveTypeDto,
        LeaveTypeDto>
    {
    }
}
