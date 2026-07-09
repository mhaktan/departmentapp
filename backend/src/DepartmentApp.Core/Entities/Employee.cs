using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    [Table("Employees")]
    public class Employee : FullAuditedEntity<long>
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

        public EmployeeGender? Gender { get; set; }

        [MaxLength(20)]
        public string NationalId { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime? TerminationDate { get; set; }

        [MaxLength(200)]
        public string JobTitle { get; set; }

        public EmployeeEmploymentType EmploymentType { get; set; }

        public EmployeeStatus Status { get; set; }

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

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        public long BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        public long EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee ParentEmployee { get; set; }

        public long OnboardingId { get; set; }

        [ForeignKey(nameof(OnboardingId))]
        public virtual Onboarding Onboarding { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<EmployeeCertificate> EmployeeCertificates { get; set; }

        public virtual ICollection<DisciplinaryRecord> DisciplinaryRecords { get; set; }

        public virtual ICollection<OvertimeRecord> OvertimeRecords { get; set; }

        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }

        public virtual ICollection<PerformanceReview> PerformanceReviews { get; set; }

        public virtual ICollection<PeerReview> PeerReviews { get; set; }

        public virtual ICollection<SalaryRecord> SalaryRecords { get; set; }

        public virtual ICollection<TrainingParticipation> TrainingParticipations { get; set; }

    }
}