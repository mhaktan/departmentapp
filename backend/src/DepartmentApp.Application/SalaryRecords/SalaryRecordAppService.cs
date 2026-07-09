using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.SalaryRecords.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.SalaryRecords
{
    public class SalaryRecordAppService : AsyncCrudAppService<
        SalaryRecord,
        SalaryRecordDto,
        long,
        PagedSalaryRecordResultRequestDto,
        CreateSalaryRecordDto,
        SalaryRecordDto>,
        ISalaryRecordAppService
    {
        private readonly IFlowEngine _flowEngine;

        public SalaryRecordAppService(IRepository<SalaryRecord, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.SalaryRecord_Read;
            GetAllPermissionName = PermissionNames.SalaryRecord_Read;
            CreatePermissionName = PermissionNames.SalaryRecord_Create;
            UpdatePermissionName = PermissionNames.SalaryRecord_Update;
            DeletePermissionName = PermissionNames.SalaryRecord_Delete;
        }

        protected override IQueryable<SalaryRecord> CreateFilteredQuery(PagedSalaryRecordResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Currency != null && x.Currency.Contains(input.Keyword)) ||
                    (x.Notes != null && x.Notes.Contains(input.Keyword)))
                .WhereIf(!input.Currency.IsNullOrWhiteSpace(), x => x.Currency != null && x.Currency.Contains(input.Currency))
                .WhereIf(!input.Notes.IsNullOrWhiteSpace(), x => x.Notes != null && x.Notes.Contains(input.Notes))
                .WhereIf(input.EffectiveDate.HasValue, x => x.EffectiveDate == input.EffectiveDate.Value)
                .WhereIf(input.GrossSalary.HasValue, x => x.GrossSalary == input.GrossSalary.Value)
                .WhereIf(input.NetSalary.HasValue, x => x.NetSalary == input.NetSalary.Value)
                .WhereIf(input.SalaryType.HasValue, x => x.SalaryType == (SalaryType)input.SalaryType.Value)
                .WhereIf(input.EmployeeId.HasValue, x => x.EmployeeId == input.EmployeeId.Value);
        }

        public override async Task<SalaryRecordDto> CreateAsync(CreateSalaryRecordDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "SalaryRecord", result);
            return result;
        }

        public override async Task<SalaryRecordDto> UpdateAsync(SalaryRecordDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "SalaryRecord", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "SalaryRecord", new { Id = input.Id });
        }
    }
}
