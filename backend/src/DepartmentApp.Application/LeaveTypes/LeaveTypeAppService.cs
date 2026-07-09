using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.LeaveTypes.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.LeaveTypes
{
    public class LeaveTypeAppService : AsyncCrudAppService<
        LeaveType,
        LeaveTypeDto,
        long,
        PagedLeaveTypeResultRequestDto,
        CreateLeaveTypeDto,
        LeaveTypeDto>,
        ILeaveTypeAppService
    {
        private readonly IFlowEngine _flowEngine;

        public LeaveTypeAppService(IRepository<LeaveType, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.LeaveType_Read;
            GetAllPermissionName = PermissionNames.LeaveType_Read;
            CreatePermissionName = PermissionNames.LeaveType_Create;
            UpdatePermissionName = PermissionNames.LeaveType_Update;
            DeletePermissionName = PermissionNames.LeaveType_Delete;
        }

        protected override IQueryable<LeaveType> CreateFilteredQuery(PagedLeaveTypeResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Name != null && x.Name.Contains(input.Keyword)) ||
                    (x.Code != null && x.Code.Contains(input.Keyword)) ||
                    (x.Description != null && x.Description.Contains(input.Keyword)))
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name != null && x.Name.Contains(input.Name))
                .WhereIf(!input.Code.IsNullOrWhiteSpace(), x => x.Code != null && x.Code.Contains(input.Code))
                .WhereIf(!input.Description.IsNullOrWhiteSpace(), x => x.Description != null && x.Description.Contains(input.Description))
                .WhereIf(input.RequiresHRApproval.HasValue, x => x.RequiresHRApproval == input.RequiresHRApproval.Value)
                .WhereIf(input.IsPaid.HasValue, x => x.IsPaid == input.IsPaid.Value)
                .WhereIf(input.MaxDaysPerYear.HasValue, x => x.MaxDaysPerYear == input.MaxDaysPerYear.Value);
        }

        public override async Task<LeaveTypeDto> CreateAsync(CreateLeaveTypeDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "LeaveType", result);
            return result;
        }

        public override async Task<LeaveTypeDto> UpdateAsync(LeaveTypeDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "LeaveType", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "LeaveType", new { Id = input.Id });
        }
    }
}
