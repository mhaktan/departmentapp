using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp.Entities;
using DepartmentApp.EmployeeCertificates.Dto;
using DepartmentApp.Authorization;
using DepartmentApp.Flows;

namespace DepartmentApp.EmployeeCertificates
{
    public class EmployeeCertificateAppService : AsyncCrudAppService<
        EmployeeCertificate,
        EmployeeCertificateDto,
        long,
        PagedEmployeeCertificateResultRequestDto,
        CreateEmployeeCertificateDto,
        EmployeeCertificateDto>,
        IEmployeeCertificateAppService
    {
        private readonly IFlowEngine _flowEngine;

        public EmployeeCertificateAppService(IRepository<EmployeeCertificate, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.EmployeeCertificate_Read;
            GetAllPermissionName = PermissionNames.EmployeeCertificate_Read;
            CreatePermissionName = PermissionNames.EmployeeCertificate_Create;
            UpdatePermissionName = PermissionNames.EmployeeCertificate_Update;
            DeletePermissionName = PermissionNames.EmployeeCertificate_Delete;
        }

        protected override IQueryable<EmployeeCertificate> CreateFilteredQuery(PagedEmployeeCertificateResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.CertificateName != null && x.CertificateName.Contains(input.Keyword)) ||
                    (x.IssuingBody != null && x.IssuingBody.Contains(input.Keyword)) ||
                    (x.CertificateNumber != null && x.CertificateNumber.Contains(input.Keyword)))
                .WhereIf(!input.CertificateName.IsNullOrWhiteSpace(), x => x.CertificateName != null && x.CertificateName.Contains(input.CertificateName))
                .WhereIf(!input.IssuingBody.IsNullOrWhiteSpace(), x => x.IssuingBody != null && x.IssuingBody.Contains(input.IssuingBody))
                .WhereIf(!input.CertificateNumber.IsNullOrWhiteSpace(), x => x.CertificateNumber != null && x.CertificateNumber.Contains(input.CertificateNumber))
                .WhereIf(input.IssueDate.HasValue, x => x.IssueDate == input.IssueDate.Value)
                .WhereIf(input.ExpiryDate.HasValue, x => x.ExpiryDate == input.ExpiryDate.Value)
                .WhereIf(input.EmployeeId.HasValue, x => x.EmployeeId == input.EmployeeId.Value);
        }

        public override async Task<EmployeeCertificateDto> CreateAsync(CreateEmployeeCertificateDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "EmployeeCertificate", result);
            return result;
        }

        public override async Task<EmployeeCertificateDto> UpdateAsync(EmployeeCertificateDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "EmployeeCertificate", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "EmployeeCertificate", new { Id = input.Id });
        }
    }
}
