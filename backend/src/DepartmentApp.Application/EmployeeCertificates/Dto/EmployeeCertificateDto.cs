using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.EmployeeCertificates.Dto
{
    [AutoMapFrom(typeof(Entities.EmployeeCertificate))]
    public class EmployeeCertificateDto : EntityDto<long>
    {
        public string CertificateName { get; set; }

        public string IssuingBody { get; set; }

        public DateTime? IssueDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string CertificateNumber { get; set; }

        public long EmployeeId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}