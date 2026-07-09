using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.EmployeeCertificates.Dto
{
    [AutoMapTo(typeof(Entities.EmployeeCertificate))]
    public class CreateEmployeeCertificateDto
    {
        [Required]
        [MaxLength(200)]
        public string CertificateName { get; set; }

        [MaxLength(200)]
        public string IssuingBody { get; set; }

        public DateTime? IssueDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [MaxLength(100)]
        public string CertificateNumber { get; set; }

        public long EmployeeId { get; set; }

    }
}