using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.JobPostings.Dto;

namespace DepartmentApp.JobPostings
{
    public interface IJobPostingAppService : IAsyncCrudAppService<
        JobPostingDto,
        long,
        PagedJobPostingResultRequestDto,
        CreateJobPostingDto,
        JobPostingDto>
    {
        Task<JobPostingDto> ChangeStatusAsync(long id, ChangeStatusInput input);
    }
}
