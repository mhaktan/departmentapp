using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.Branchs.Dto;

namespace DepartmentApp.Branchs
{
    public interface IBranchAppService : IAsyncCrudAppService<
        BranchDto,
        long,
        PagedBranchResultRequestDto,
        CreateBranchDto,
        BranchDto>
    {
    }
}
