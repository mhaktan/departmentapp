using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.Branchs.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.Branchs
{
    public class BranchAppService : AsyncCrudAppService<
        Branch,
        BranchDto,
        long,
        PagedBranchResultRequestDto,
        CreateBranchDto,
        BranchDto>,
        IBranchAppService
    {
        private readonly IFlowEngine _flowEngine;

        public BranchAppService(IRepository<Branch, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.Branch_Read;
            GetAllPermissionName = PermissionNames.Branch_Read;
            CreatePermissionName = PermissionNames.Branch_Create;
            UpdatePermissionName = PermissionNames.Branch_Update;
            DeletePermissionName = PermissionNames.Branch_Delete;
        }

        protected override IQueryable<Branch> CreateFilteredQuery(PagedBranchResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Name != null && x.Name.Contains(input.Keyword)) ||
                    (x.Address != null && x.Address.Contains(input.Keyword)) ||
                    (x.Phone != null && x.Phone.Contains(input.Keyword)))
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name != null && x.Name.Contains(input.Name))
                .WhereIf(!input.Address.IsNullOrWhiteSpace(), x => x.Address != null && x.Address.Contains(input.Address))
                .WhereIf(!input.Phone.IsNullOrWhiteSpace(), x => x.Phone != null && x.Phone.Contains(input.Phone));
        }

        public override async Task<BranchDto> CreateAsync(CreateBranchDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "Branch", result);
            return result;
        }

        public override async Task<BranchDto> UpdateAsync(BranchDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "Branch", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "Branch", new { Id = input.Id });
        }
    }
}
