using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.JobApplications.Dto;

namespace DepartmentApp.JobApplications
{
    public interface IJobApplicationAppService : IAsyncCrudAppService<
        JobApplicationDto,
        long,
        PagedJobApplicationResultRequestDto,
        CreateJobApplicationDto,
        JobApplicationDto>
    {
        Task<JobApplicationDto> ChangeStatusAsync(long id, ChangeStatusInput input);
    }
}
