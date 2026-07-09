using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.PerformanceGoals.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.PerformanceGoals
{
    public class PerformanceGoalAppService : AsyncCrudAppService<
        PerformanceGoal,
        PerformanceGoalDto,
        long,
        PagedPerformanceGoalResultRequestDto,
        CreatePerformanceGoalDto,
        PerformanceGoalDto>,
        IPerformanceGoalAppService
    {
        private readonly IFlowEngine _flowEngine;

        public PerformanceGoalAppService(IRepository<PerformanceGoal, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.PerformanceGoal_Read;
            GetAllPermissionName = PermissionNames.PerformanceGoal_Read;
            CreatePermissionName = PermissionNames.PerformanceGoal_Create;
            UpdatePermissionName = PermissionNames.PerformanceGoal_Update;
            DeletePermissionName = PermissionNames.PerformanceGoal_Delete;
        }

        protected override IQueryable<PerformanceGoal> CreateFilteredQuery(PagedPerformanceGoalResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Title != null && x.Title.Contains(input.Keyword)) ||
                    (x.Description != null && x.Description.Contains(input.Keyword)))
                .WhereIf(!input.Title.IsNullOrWhiteSpace(), x => x.Title != null && x.Title.Contains(input.Title))
                .WhereIf(!input.Description.IsNullOrWhiteSpace(), x => x.Description != null && x.Description.Contains(input.Description))
                .WhereIf(input.TargetDate.HasValue, x => x.TargetDate == input.TargetDate.Value)
                .WhereIf(input.Weight.HasValue, x => x.Weight == input.Weight.Value)
                .WhereIf(input.SelfScore.HasValue, x => x.SelfScore == input.SelfScore.Value)
                .WhereIf(input.ManagerScore.HasValue, x => x.ManagerScore == input.ManagerScore.Value)
                .WhereIf(input.Status.HasValue, x => x.Status == (PerformanceGoalStatus)input.Status.Value)
                .WhereIf(input.PerformanceReviewId.HasValue, x => x.PerformanceReviewId == input.PerformanceReviewId.Value);
        }

        public override async Task<PerformanceGoalDto> CreateAsync(CreatePerformanceGoalDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "PerformanceGoal", result);
            return result;
        }

        public override async Task<PerformanceGoalDto> UpdateAsync(PerformanceGoalDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "PerformanceGoal", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "PerformanceGoal", new { Id = input.Id });
        }
    }
}
