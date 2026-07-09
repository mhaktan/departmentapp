using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.LeaveRequests.Dto;

namespace DepartmentApp.LeaveRequests
{
    public interface ILeaveRequestAppService : IAsyncCrudAppService<
        LeaveRequestDto,
        long,
        PagedLeaveRequestResultRequestDto,
        CreateLeaveRequestDto,
        LeaveRequestDto>
    {
        Task<LeaveRequestDto> ChangeStatusAsync(long id, ChangeStatusInput input);
    }
}
