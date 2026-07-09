using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.EmployeeCertificates.Dto
{
    public class PagedEmployeeCertificateResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? EmployeeId { get; set; }
        public string CertificateName { get; set; }
        public string IssuingBody { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CertificateNumber { get; set; }
    }
}
