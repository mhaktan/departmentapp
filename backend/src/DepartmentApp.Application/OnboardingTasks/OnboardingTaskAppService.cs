using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.OnboardingTasks.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.OnboardingTasks
{
    public class OnboardingTaskAppService : AsyncCrudAppService<
        OnboardingTask,
        OnboardingTaskDto,
        long,
        PagedOnboardingTaskResultRequestDto,
        CreateOnboardingTaskDto,
        OnboardingTaskDto>,
        IOnboardingTaskAppService
    {
        private readonly IFlowEngine _flowEngine;

        public OnboardingTaskAppService(IRepository<OnboardingTask, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.OnboardingTask_Read;
            GetAllPermissionName = PermissionNames.OnboardingTask_Read;
            CreatePermissionName = PermissionNames.OnboardingTask_Create;
            UpdatePermissionName = PermissionNames.OnboardingTask_Update;
            DeletePermissionName = PermissionNames.OnboardingTask_Delete;
        }

        protected override IQueryable<OnboardingTask> CreateFilteredQuery(PagedOnboardingTaskResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Title != null && x.Title.Contains(input.Keyword)) ||
                    (x.Description != null && x.Description.Contains(input.Keyword)) ||
                    (x.AssignedTo != null && x.AssignedTo.Contains(input.Keyword)))
                .WhereIf(!input.Title.IsNullOrWhiteSpace(), x => x.Title != null && x.Title.Contains(input.Title))
                .WhereIf(!input.Description.IsNullOrWhiteSpace(), x => x.Description != null && x.Description.Contains(input.Description))
                .WhereIf(!input.AssignedTo.IsNullOrWhiteSpace(), x => x.AssignedTo != null && x.AssignedTo.Contains(input.AssignedTo))
                .WhereIf(input.IsCompleted.HasValue, x => x.IsCompleted == input.IsCompleted.Value)
                .WhereIf(input.CompletedDate.HasValue, x => x.CompletedDate == input.CompletedDate.Value)
                .WhereIf(input.DueDate.HasValue, x => x.DueDate == input.DueDate.Value)
                .WhereIf(input.OnboardingId.HasValue, x => x.OnboardingId == input.OnboardingId.Value);
        }

        public override async Task<OnboardingTaskDto> CreateAsync(CreateOnboardingTaskDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "OnboardingTask", result);
            return result;
        }

        public override async Task<OnboardingTaskDto> UpdateAsync(OnboardingTaskDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "OnboardingTask", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "OnboardingTask", new { Id = input.Id });
        }
    }
}
