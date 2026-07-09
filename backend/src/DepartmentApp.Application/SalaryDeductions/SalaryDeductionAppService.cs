using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.SalaryDeductions.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.SalaryDeductions
{
    public class SalaryDeductionAppService : AsyncCrudAppService<
        SalaryDeduction,
        SalaryDeductionDto,
        long,
        PagedSalaryDeductionResultRequestDto,
        CreateSalaryDeductionDto,
        SalaryDeductionDto>,
        ISalaryDeductionAppService
    {
        private readonly IFlowEngine _flowEngine;

        public SalaryDeductionAppService(IRepository<SalaryDeduction, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.SalaryDeduction_Read;
            GetAllPermissionName = PermissionNames.SalaryDeduction_Read;
            CreatePermissionName = PermissionNames.SalaryDeduction_Create;
            UpdatePermissionName = PermissionNames.SalaryDeduction_Update;
            DeletePermissionName = PermissionNames.SalaryDeduction_Delete;
        }

        protected override IQueryable<SalaryDeduction> CreateFilteredQuery(PagedSalaryDeductionResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.DeductionType != null && x.DeductionType.Contains(input.Keyword)) ||
                    (x.Currency != null && x.Currency.Contains(input.Keyword)) ||
                    (x.Description != null && x.Description.Contains(input.Keyword)))
                .WhereIf(!input.DeductionType.IsNullOrWhiteSpace(), x => x.DeductionType != null && x.DeductionType.Contains(input.DeductionType))
                .WhereIf(!input.Currency.IsNullOrWhiteSpace(), x => x.Currency != null && x.Currency.Contains(input.Currency))
                .WhereIf(!input.Description.IsNullOrWhiteSpace(), x => x.Description != null && x.Description.Contains(input.Description))
                .WhereIf(input.Amount.HasValue, x => x.Amount == input.Amount.Value)
                .WhereIf(input.EffectiveDate.HasValue, x => x.EffectiveDate == input.EffectiveDate.Value)
                .WhereIf(input.SalaryRecordId.HasValue, x => x.SalaryRecordId == input.SalaryRecordId.Value);
        }

        public override async Task<SalaryDeductionDto> CreateAsync(CreateSalaryDeductionDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "SalaryDeduction", result);
            return result;
        }

        public override async Task<SalaryDeductionDto> UpdateAsync(SalaryDeductionDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "SalaryDeduction", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "SalaryDeduction", new { Id = input.Id });
        }
    }
}
