using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.Employees.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.Employees
{
    public class EmployeeAppService : AsyncCrudAppService<
        Employee,
        EmployeeDto,
        long,
        PagedEmployeeResultRequestDto,
        CreateEmployeeDto,
        EmployeeDto>,
        IEmployeeAppService
    {
        private readonly IFlowEngine _flowEngine;

        public EmployeeAppService(IRepository<Employee, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.Employee_Read;
            GetAllPermissionName = PermissionNames.Employee_Read;
            CreatePermissionName = PermissionNames.Employee_Create;
            UpdatePermissionName = PermissionNames.Employee_Update;
            DeletePermissionName = PermissionNames.Employee_Delete;
        }

        protected override IQueryable<Employee> CreateFilteredQuery(PagedEmployeeResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.EmployeeNumber != null && x.EmployeeNumber.Contains(input.Keyword)) ||
                    (x.FirstName != null && x.FirstName.Contains(input.Keyword)) ||
                    (x.LastName != null && x.LastName.Contains(input.Keyword)) ||
                    (x.Email != null && x.Email.Contains(input.Keyword)) ||
                    (x.Phone != null && x.Phone.Contains(input.Keyword)) ||
                    (x.NationalId != null && x.NationalId.Contains(input.Keyword)) ||
                    (x.Address != null && x.Address.Contains(input.Keyword)) ||
                    (x.JobTitle != null && x.JobTitle.Contains(input.Keyword)) ||
                    (x.EmergencyContactName != null && x.EmergencyContactName.Contains(input.Keyword)) ||
                    (x.EmergencyContactPhone != null && x.EmergencyContactPhone.Contains(input.Keyword)) ||
                    (x.EmergencyContactRelation != null && x.EmergencyContactRelation.Contains(input.Keyword)) ||
                    (x.BankAccountNumber != null && x.BankAccountNumber.Contains(input.Keyword)) ||
                    (x.BankName != null && x.BankName.Contains(input.Keyword)) ||
                    (x.TaxNumber != null && x.TaxNumber.Contains(input.Keyword)) ||
                    (x.SocialSecurityNumber != null && x.SocialSecurityNumber.Contains(input.Keyword)) ||
                    (x.Notes != null && x.Notes.Contains(input.Keyword)))
                .WhereIf(!input.EmployeeNumber.IsNullOrWhiteSpace(), x => x.EmployeeNumber != null && x.EmployeeNumber.Contains(input.EmployeeNumber))
                .WhereIf(!input.FirstName.IsNullOrWhiteSpace(), x => x.FirstName != null && x.FirstName.Contains(input.FirstName))
                .WhereIf(!input.LastName.IsNullOrWhiteSpace(), x => x.LastName != null && x.LastName.Contains(input.LastName))
                .WhereIf(!input.Email.IsNullOrWhiteSpace(), x => x.Email != null && x.Email.Contains(input.Email))
                .WhereIf(!input.Phone.IsNullOrWhiteSpace(), x => x.Phone != null && x.Phone.Contains(input.Phone))
                .WhereIf(!input.NationalId.IsNullOrWhiteSpace(), x => x.NationalId != null && x.NationalId.Contains(input.NationalId))
                .WhereIf(!input.Address.IsNullOrWhiteSpace(), x => x.Address != null && x.Address.Contains(input.Address))
                .WhereIf(!input.JobTitle.IsNullOrWhiteSpace(), x => x.JobTitle != null && x.JobTitle.Contains(input.JobTitle))
                .WhereIf(!input.EmergencyContactName.IsNullOrWhiteSpace(), x => x.EmergencyContactName != null && x.EmergencyContactName.Contains(input.EmergencyContactName))
                .WhereIf(!input.EmergencyContactPhone.IsNullOrWhiteSpace(), x => x.EmergencyContactPhone != null && x.EmergencyContactPhone.Contains(input.EmergencyContactPhone))
                .WhereIf(!input.EmergencyContactRelation.IsNullOrWhiteSpace(), x => x.EmergencyContactRelation != null && x.EmergencyContactRelation.Contains(input.EmergencyContactRelation))
                .WhereIf(!input.BankAccountNumber.IsNullOrWhiteSpace(), x => x.BankAccountNumber != null && x.BankAccountNumber.Contains(input.BankAccountNumber))
                .WhereIf(!input.BankName.IsNullOrWhiteSpace(), x => x.BankName != null && x.BankName.Contains(input.BankName))
                .WhereIf(!input.TaxNumber.IsNullOrWhiteSpace(), x => x.TaxNumber != null && x.TaxNumber.Contains(input.TaxNumber))
                .WhereIf(!input.SocialSecurityNumber.IsNullOrWhiteSpace(), x => x.SocialSecurityNumber != null && x.SocialSecurityNumber.Contains(input.SocialSecurityNumber))
                .WhereIf(!input.Notes.IsNullOrWhiteSpace(), x => x.Notes != null && x.Notes.Contains(input.Notes))
                .WhereIf(input.BirthDate.HasValue, x => x.BirthDate == input.BirthDate.Value)
                .WhereIf(input.Gender.HasValue, x => x.Gender == (Gender)input.Gender.Value)
                .WhereIf(input.HireDate.HasValue, x => x.HireDate == input.HireDate.Value)
                .WhereIf(input.TerminationDate.HasValue, x => x.TerminationDate == input.TerminationDate.Value)
                .WhereIf(input.EmploymentType.HasValue, x => x.EmploymentType == (EmploymentType)input.EmploymentType.Value)
                .WhereIf(input.Status.HasValue, x => x.Status == (Status)input.Status.Value)
                .WhereIf(input.AnnualLeaveBalance.HasValue, x => x.AnnualLeaveBalance == input.AnnualLeaveBalance.Value)
                .WhereIf(input.DepartmentId.HasValue, x => x.DepartmentId == input.DepartmentId.Value)
                .WhereIf(input.BranchId.HasValue, x => x.BranchId == input.BranchId.Value)
                .WhereIf(input.EmployeeId.HasValue, x => x.EmployeeId == input.EmployeeId.Value)
                .WhereIf(input.OnboardingId.HasValue, x => x.OnboardingId == input.OnboardingId.Value);
        }

        public override async Task<EmployeeDto> CreateAsync(CreateEmployeeDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "Employee", result);
            return result;
        }

        public override async Task<EmployeeDto> UpdateAsync(EmployeeDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "Employee", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "Employee", new { Id = input.Id });
        }
    }
}
