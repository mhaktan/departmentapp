using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.Employees.Dto
{
    public class PagedEmployeeResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? DepartmentId { get; set; }
        public long? BranchId { get; set; }
        public long? EmployeeId { get; set; }
        public long? OnboardingId { get; set; }
        public string EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Gender { get; set; }
        public string NationalId { get; set; }
        public string Address { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string JobTitle { get; set; }
        public int? EmploymentType { get; set; }
        public int? Status { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactPhone { get; set; }
        public string EmergencyContactRelation { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string TaxNumber { get; set; }
        public string SocialSecurityNumber { get; set; }
        public decimal? AnnualLeaveBalance { get; set; }
        public string Notes { get; set; }
    }
}
