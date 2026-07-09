using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.Employees.Dto
{
    [AutoMapTo(typeof(Entities.Employee))]
    public class CreateEmployeeDto
    {
        [Required]
        [MaxLength(50)]
        public string EmployeeNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? Gender { get; set; }

        [MaxLength(20)]
        public string NationalId { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime? TerminationDate { get; set; }

        [MaxLength(200)]
        public string JobTitle { get; set; }

        public int EmploymentType { get; set; }

        public int Status { get; set; }

        [MaxLength(200)]
        public string EmergencyContactName { get; set; }

        [MaxLength(50)]
        public string EmergencyContactPhone { get; set; }

        [MaxLength(100)]
        public string EmergencyContactRelation { get; set; }

        [MaxLength(50)]
        public string BankAccountNumber { get; set; }

        [MaxLength(200)]
        public string BankName { get; set; }

        [MaxLength(50)]
        public string TaxNumber { get; set; }

        [MaxLength(50)]
        public string SocialSecurityNumber { get; set; }

        public decimal? AnnualLeaveBalance { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

        public long DepartmentId { get; set; }

        public long BranchId { get; set; }

        public long EmployeeId { get; set; }

        public long OnboardingId { get; set; }

    }
}