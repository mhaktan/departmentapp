using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp.EmployeeCertificates.Dto;

namespace DepartmentApp.EmployeeCertificates
{
    public interface IEmployeeCertificateAppService : IAsyncCrudAppService<
        EmployeeCertificateDto,
        long,
        PagedEmployeeCertificateResultRequestDto,
        CreateEmployeeCertificateDto,
        EmployeeCertificateDto>
    {
    }
}
