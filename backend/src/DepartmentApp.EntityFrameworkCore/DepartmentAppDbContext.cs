using Microsoft.EntityFrameworkCore;
using Abp.EntityFrameworkCore;
using DepartmentApp.Entities;

namespace DepartmentApp.EntityFrameworkCore
{
    public class DepartmentAppDbContext : AbpDbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeCertificate> EmployeeCertificates { get; set; }
        public DbSet<DisciplinaryRecord> DisciplinaryRecords { get; set; }
        public DbSet<OvertimeRecord> OvertimeRecords { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<PerformanceReview> PerformanceReviews { get; set; }
        public DbSet<PerformanceGoal> PerformanceGoals { get; set; }
        public DbSet<PeerReview> PeerReviews { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<Onboarding> Onboardings { get; set; }
        public DbSet<OnboardingTask> OnboardingTasks { get; set; }
        public DbSet<SalaryRecord> SalaryRecords { get; set; }
        public DbSet<SalaryDeduction> SalaryDeductions { get; set; }
        public DbSet<TrainingPlan> TrainingPlans { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<TrainingParticipation> TrainingParticipations { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<ApprovalRecord> ApprovalRecords { get; set; }
        public DbSet<StatusChangeLog> StatusChangeLogs { get; set; }


        public DepartmentAppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Branch 1:N Department
            modelBuilder.Entity<Department>()
                .HasOne(x => x.Branch)
                .WithMany(x => x.Departments)
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Department 1:N Employee
            modelBuilder.Entity<Employee>()
                .HasOne(x => x.Department)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Branch 1:N Employee
            modelBuilder.Entity<Employee>()
                .HasOne(x => x.Branch)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee 1:N Employee
            modelBuilder.Entity<Employee>()
                .HasOne(x => x.ParentEmployee)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee 1:N EmployeeCertificate
            modelBuilder.Entity<EmployeeCertificate>()
                .HasOne(x => x.Employee)
                .WithMany(x => x.EmployeeCertificates)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Employee 1:N DisciplinaryRecord
            modelBuilder.Entity<DisciplinaryRecord>()
                .HasOne(x => x.Employee)
                .WithMany(x => x.DisciplinaryRecords)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee 1:N OvertimeRecord
            modelBuilder.Entity<OvertimeRecord>()
                .HasOne(x => x.Employee)
                .WithMany(x => x.OvertimeRecords)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee 1:N LeaveRequest
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(x => x.Employee)
                .WithMany(x => x.LeaveRequests)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // LeaveType 1:N LeaveRequest
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(x => x.LeaveType)
                .WithMany(x => x.LeaveRequests)
                .HasForeignKey(x => x.LeaveTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee 1:N PerformanceReview
            modelBuilder.Entity<PerformanceReview>()
                .HasOne(x => x.Employee)
                .WithMany(x => x.PerformanceReviews)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // PerformanceReview 1:N PerformanceGoal
            modelBuilder.Entity<PerformanceGoal>()
                .HasOne(x => x.PerformanceReview)
                .WithMany(x => x.PerformanceGoals)
                .HasForeignKey(x => x.PerformanceReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            // PerformanceReview 1:N PeerReview
            modelBuilder.Entity<PeerReview>()
                .HasOne(x => x.PerformanceReview)
                .WithMany(x => x.PeerReviews)
                .HasForeignKey(x => x.PerformanceReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            // Employee 1:N PeerReview
            modelBuilder.Entity<PeerReview>()
                .HasOne(x => x.Employee)
                .WithMany(x => x.PeerReviews)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Department 1:N JobPosting
            modelBuilder.Entity<JobPosting>()
                .HasOne(x => x.Department)
                .WithMany(x => x.JobPostings)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // JobPosting 1:N JobApplication
            modelBuilder.Entity<JobApplication>()
                .HasOne(x => x.JobPosting)
                .WithMany(x => x.JobApplications)
                .HasForeignKey(x => x.JobPostingId)
                .OnDelete(DeleteBehavior.Restrict);

            // JobApplication 1:1 Onboarding
            modelBuilder.Entity<Onboarding>()
                .HasOne(x => x.JobApplication)
                .WithOne()
                .HasForeignKey<Onboarding>(x => x.JobApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Onboarding 1:1 Employee
            modelBuilder.Entity<Employee>()
                .HasOne(x => x.Onboarding)
                .WithOne()
                .HasForeignKey<Employee>(x => x.OnboardingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Onboarding 1:N OnboardingTask
            modelBuilder.Entity<OnboardingTask>()
                .HasOne(x => x.Onboarding)
                .WithMany(x => x.OnboardingTasks)
                .HasForeignKey(x => x.OnboardingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Employee 1:N SalaryRecord
            modelBuilder.Entity<SalaryRecord>()
                .HasOne(x => x.Employee)
                .WithMany(x => x.SalaryRecords)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // SalaryRecord 1:N SalaryDeduction
            modelBuilder.Entity<SalaryDeduction>()
                .HasOne(x => x.SalaryRecord)
                .WithMany(x => x.SalaryDeductions)
                .HasForeignKey(x => x.SalaryRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            // Department 1:N TrainingPlan
            modelBuilder.Entity<TrainingPlan>()
                .HasOne(x => x.Department)
                .WithMany(x => x.TrainingPlans)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // TrainingPlan 1:N Training
            modelBuilder.Entity<Training>()
                .HasOne(x => x.TrainingPlan)
                .WithMany(x => x.Trainings)
                .HasForeignKey(x => x.TrainingPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // Training 1:N TrainingParticipation
            modelBuilder.Entity<TrainingParticipation>()
                .HasOne(x => x.Training)
                .WithMany(x => x.TrainingParticipations)
                .HasForeignKey(x => x.TrainingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee 1:N TrainingParticipation
            modelBuilder.Entity<TrainingParticipation>()
                .HasOne(x => x.Employee)
                .WithMany(x => x.TrainingParticipations)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);


            // RBAC: AppUser N:N AppRole via UserRole junction
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserRole>()
                .HasIndex(ur => new { ur.UserId, ur.RoleId })
                .IsUnique();

            // RolePermission: AppRole 1:N RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RolePermission>()
                .HasIndex(rp => new { rp.RoleId, rp.PermissionName })
                .IsUnique();

            // AppRole.Name unique
            modelBuilder.Entity<AppRole>()
                .HasIndex(r => r.Name)
                .IsUnique();

        }
    }
}
