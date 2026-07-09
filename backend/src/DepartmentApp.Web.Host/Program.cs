using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DepartmentApp.EntityFrameworkCore;
using DepartmentApp.EntityFrameworkCore.Seed;
using DepartmentApp.Entities;

namespace DepartmentApp.Web.Host
{
    /// <summary>
    /// Background service that runs migration + seed once at startup
    /// without blocking the HTTP pipeline.
    /// </summary>
    public class MigrationHostedService : IHostedService
    {
        private readonly IConfiguration _config;

        public MigrationHostedService(IConfiguration config)
        {
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var connStr = _config.GetConnectionString("Default") ?? "";
            if (string.IsNullOrEmpty(connStr)) return;

            // Run in background to avoid blocking host startup (prevents EF tooling timeout)
            _ = Task.Run(async () =>
            {
                // Small delay to ensure host is fully started before DB operations
                await Task.Delay(1000, cancellationToken);
                try
                {
                    var optionsBuilder = new DbContextOptionsBuilder();
                    optionsBuilder.UseNpgsql(connStr);

                    using (var db = new DepartmentAppDbContext(optionsBuilder.Options))
                    {
                        db.Database.EnsureCreated();
                        Console.WriteLine("[Migration] Database is up to date.");

                        // Seed sample data — wrapped in its own try so a failure here doesn't block RBAC seed below.
                        try
                        {
                    if (!db.Departments.Any())
                    {
                        db.Departments.AddRange(
                    new Department { Id = 3, Name = "Alice Johnson", Code = "ABC-001", Description = "Lorem ipsum dolor sit amet", BranchId = 1 },
                    new Department { Id = 4, Name = "Bob Smith", Code = "XYZ-002", Description = "Consectetur adipiscing elit", BranchId = 2 }
                        );
                    }
                    if (!db.Branches.Any())
                    {
                        db.Branches.AddRange(
                    new Branch { Id = 1, Name = "Alice Johnson", Address = "123 Main St, New York", Phone = "+1-555-0101" },
                    new Branch { Id = 2, Name = "Bob Smith", Address = "456 Oak Ave, London", Phone = "+1-555-0102" }
                        );
                    }
                    if (!db.Employees.Any())
                    {
                        db.Employees.AddRange(
                    new Employee { Id = 11, EmployeeNumber = "ABC-001", FirstName = "Alice Johnson", LastName = "Alice Johnson", Email = "alice@example.com", Phone = "+1-555-0101", BirthDate = new DateTime(2024, 3, 15), Gender = (EmployeeGender)0, NationalId = "Sample Item 1", Address = "123 Main St, New York", HireDate = new DateTime(2024, 3, 15), TerminationDate = new DateTime(2024, 3, 15), JobTitle = "Introduction to Physics", EmploymentType = (EmployeeEmploymentType)0, Status = (EmployeeStatus)0, EmergencyContactName = "Alice Johnson", EmergencyContactPhone = "+1-555-0101", EmergencyContactRelation = "Sample Item 1", BankAccountNumber = "ABC-001", BankName = "Alice Johnson", TaxNumber = "ABC-001", SocialSecurityNumber = "ABC-001", AnnualLeaveBalance = 99.99m, Notes = "Lorem ipsum dolor sit amet", DepartmentId = 3, BranchId = 1, EmployeeId = 11, OnboardingId = 9 },
                    new Employee { Id = 12, EmployeeNumber = "XYZ-002", FirstName = "Bob Smith", LastName = "Bob Smith", Email = "bob@example.com", Phone = "+1-555-0102", BirthDate = new DateTime(2024, 6, 20), Gender = (EmployeeGender)1, NationalId = "Sample Item 2", Address = "456 Oak Ave, London", HireDate = new DateTime(2024, 6, 20), TerminationDate = new DateTime(2024, 6, 20), JobTitle = "Advanced Mathematics", EmploymentType = (EmployeeEmploymentType)1, Status = (EmployeeStatus)1, EmergencyContactName = "Bob Smith", EmergencyContactPhone = "+1-555-0102", EmergencyContactRelation = "Sample Item 2", BankAccountNumber = "XYZ-002", BankName = "Bob Smith", TaxNumber = "XYZ-002", SocialSecurityNumber = "XYZ-002", AnnualLeaveBalance = 149.50m, Notes = "Consectetur adipiscing elit", DepartmentId = 4, BranchId = 2, EmployeeId = 12, OnboardingId = 10 }
                        );
                    }
                    if (!db.EmployeeCertificates.Any())
                    {
                        db.EmployeeCertificates.AddRange(
                    new EmployeeCertificate { Id = 13, CertificateName = "Alice Johnson", IssuingBody = "Sample Item 1", IssueDate = new DateTime(2024, 3, 15), ExpiryDate = new DateTime(2024, 3, 15), CertificateNumber = "ABC-001", EmployeeId = 11 },
                    new EmployeeCertificate { Id = 14, CertificateName = "Bob Smith", IssuingBody = "Sample Item 2", IssueDate = new DateTime(2024, 6, 20), ExpiryDate = new DateTime(2024, 6, 20), CertificateNumber = "XYZ-002", EmployeeId = 12 }
                        );
                    }
                    if (!db.DisciplinaryRecords.Any())
                    {
                        db.DisciplinaryRecords.AddRange(
                    new DisciplinaryRecord { Id = 15, IncidentDate = new DateTime(2024, 3, 15), Type = (DisciplinaryRecordType)0, Description = "Lorem ipsum dolor sit amet", ActionTaken = "Sample Item 1", IssuedBy = "Sample Item 1", AcknowledgedByEmployee = true, Status = (DisciplinaryRecordStatus)0, AppealNote = "Lorem ipsum dolor sit amet", ResolutionNote = "Lorem ipsum dolor sit amet", HrReviewerId = 1000L, HrManagerResolverId = 1000L, EmployeeId = 11 },
                    new DisciplinaryRecord { Id = 16, IncidentDate = new DateTime(2024, 6, 20), Type = (DisciplinaryRecordType)1, Description = "Consectetur adipiscing elit", ActionTaken = "Sample Item 2", IssuedBy = "Sample Item 2", AcknowledgedByEmployee = false, Status = (DisciplinaryRecordStatus)1, AppealNote = "Consectetur adipiscing elit", ResolutionNote = "Consectetur adipiscing elit", HrReviewerId = 2000L, HrManagerResolverId = 2000L, EmployeeId = 12 }
                        );
                    }
                    if (!db.OvertimeRecords.Any())
                    {
                        db.OvertimeRecords.AddRange(
                    new OvertimeRecord { Id = 17, OvertimeDate = new DateTime(2024, 3, 15), Hours = 99.99m, Reason = "Sample Item 1", Status = (OvertimeRecordStatus)0, ApproverNote = "Lorem ipsum dolor sit amet", Notes = "Lorem ipsum dolor sit amet", ManagerApproverId = 1000L, EmployeeId = 11 },
                    new OvertimeRecord { Id = 18, OvertimeDate = new DateTime(2024, 6, 20), Hours = 149.50m, Reason = "Sample Item 2", Status = (OvertimeRecordStatus)1, ApproverNote = "Consectetur adipiscing elit", Notes = "Consectetur adipiscing elit", ManagerApproverId = 2000L, EmployeeId = 12 }
                        );
                    }
                    if (!db.LeaveTypes.Any())
                    {
                        db.LeaveTypes.AddRange(
                    new LeaveType { Id = 19, Name = "Alice Johnson", Code = "ABC-001", RequiresHRApproval = true, IsPaid = true, MaxDaysPerYear = 42, Description = "Lorem ipsum dolor sit amet" },
                    new LeaveType { Id = 20, Name = "Bob Smith", Code = "XYZ-002", RequiresHRApproval = false, IsPaid = false, MaxDaysPerYear = 17, Description = "Consectetur adipiscing elit" }
                        );
                    }
                    if (!db.LeaveRequests.Any())
                    {
                        db.LeaveRequests.AddRange(
                    new LeaveRequest { Id = 21, StartDate = new DateTime(2024, 3, 15), EndDate = new DateTime(2024, 3, 15), TotalDays = 99.99m, Reason = "Sample Item 1", Status = (LeaveRequestStatus)0, RevisionNote = "Lorem ipsum dolor sit amet", RequiresHRApproval = true, ManagerApproverId = 1000L, HrApproverId = 1000L, BalanceDeducted = true, EmployeeId = 11, LeaveTypeId = 19 },
                    new LeaveRequest { Id = 22, StartDate = new DateTime(2024, 6, 20), EndDate = new DateTime(2024, 6, 20), TotalDays = 149.50m, Reason = "Sample Item 2", Status = (LeaveRequestStatus)1, RevisionNote = "Consectetur adipiscing elit", RequiresHRApproval = false, ManagerApproverId = 2000L, HrApproverId = 2000L, BalanceDeducted = false, EmployeeId = 12, LeaveTypeId = 20 }
                        );
                    }
                    if (!db.PerformanceReviews.Any())
                    {
                        db.PerformanceReviews.AddRange(
                    new PerformanceReview { Id = 23, ReviewPeriod = "Sample Item 1", ReviewYear = 42, ReviewType = (PerformanceReviewReviewType)0, Status = (PerformanceReviewStatus)0, SelfAssessmentScore = 99.99m, SelfAssessmentNotes = "Lorem ipsum dolor sit amet", ManagerScore = 99.99m, ManagerNotes = "Lorem ipsum dolor sit amet", OverallScore = 99.99m, HrNotes = "Lorem ipsum dolor sit amet", RevisionNote = "Lorem ipsum dolor sit amet", ManagerReviewerId = 1000L, HrReviewerId = 1000L, PeerReviewersAssignedBy = 1000L, EmployeeId = 11 },
                    new PerformanceReview { Id = 24, ReviewPeriod = "Sample Item 2", ReviewYear = 17, ReviewType = (PerformanceReviewReviewType)1, Status = (PerformanceReviewStatus)1, SelfAssessmentScore = 149.50m, SelfAssessmentNotes = "Consectetur adipiscing elit", ManagerScore = 149.50m, ManagerNotes = "Consectetur adipiscing elit", OverallScore = 149.50m, HrNotes = "Consectetur adipiscing elit", RevisionNote = "Consectetur adipiscing elit", ManagerReviewerId = 2000L, HrReviewerId = 2000L, PeerReviewersAssignedBy = 2000L, EmployeeId = 12 }
                        );
                    }
                    if (!db.PerformanceGoals.Any())
                    {
                        db.PerformanceGoals.AddRange(
                    new PerformanceGoal { Id = 25, Title = "Introduction to Physics", Description = "Lorem ipsum dolor sit amet", TargetDate = new DateTime(2024, 3, 15), Weight = 99.99m, SelfScore = 99.99m, ManagerScore = 99.99m, Status = (PerformanceGoalStatus)0, PerformanceReviewId = 23 },
                    new PerformanceGoal { Id = 26, Title = "Advanced Mathematics", Description = "Consectetur adipiscing elit", TargetDate = new DateTime(2024, 6, 20), Weight = 149.50m, SelfScore = 149.50m, ManagerScore = 149.50m, Status = (PerformanceGoalStatus)1, PerformanceReviewId = 24 }
                        );
                    }
                    if (!db.PeerReviews.Any())
                    {
                        db.PeerReviews.AddRange(
                    new PeerReview { Id = 27, ReviewerName = "Alice Johnson", Score = 99.99m, Strengths = "Sample Item 1", Improvements = "Sample Item 1", IsAnonymous = true, PerformanceReviewId = 23, EmployeeId = 11 },
                    new PeerReview { Id = 28, ReviewerName = "Bob Smith", Score = 149.50m, Strengths = "Sample Item 2", Improvements = "Sample Item 2", IsAnonymous = false, PerformanceReviewId = 24, EmployeeId = 12 }
                        );
                    }
                    if (!db.JobPostings.Any())
                    {
                        db.JobPostings.AddRange(
                    new JobPosting { Id = 5, Title = "Introduction to Physics", Description = "Lorem ipsum dolor sit amet", Requirements = "Sample Item 1", PositionCount = 42, Status = (JobPostingStatus)0, PublishDate = new DateTime(2024, 3, 15), ClosingDate = new DateTime(2024, 3, 15), EmploymentType = (JobPostingEmploymentType)0, DepartmentId = 3 },
                    new JobPosting { Id = 6, Title = "Advanced Mathematics", Description = "Consectetur adipiscing elit", Requirements = "Sample Item 2", PositionCount = 17, Status = (JobPostingStatus)1, PublishDate = new DateTime(2024, 6, 20), ClosingDate = new DateTime(2024, 6, 20), EmploymentType = (JobPostingEmploymentType)1, DepartmentId = 4 }
                        );
                    }
                    if (!db.JobApplications.Any())
                    {
                        db.JobApplications.AddRange(
                    new JobApplication { Id = 7, ApplicantFirstName = "Alice Johnson", ApplicantLastName = "Alice Johnson", ApplicantEmail = "alice@example.com", ApplicantPhone = "+1-555-0101", CoverLetter = "Sample Item 1", Status = (JobApplicationStatus)0, ScreeningNotes = "Lorem ipsum dolor sit amet", InterviewDate = new DateTime(2024, 3, 15), InterviewNotes = "Lorem ipsum dolor sit amet", OfferSalary = 99.99m, OfferDate = new DateTime(2024, 3, 15), RejectionReason = "Sample Item 1", JobPostingId = 5 },
                    new JobApplication { Id = 8, ApplicantFirstName = "Bob Smith", ApplicantLastName = "Bob Smith", ApplicantEmail = "bob@example.com", ApplicantPhone = "+1-555-0102", CoverLetter = "Sample Item 2", Status = (JobApplicationStatus)1, ScreeningNotes = "Consectetur adipiscing elit", InterviewDate = new DateTime(2024, 6, 20), InterviewNotes = "Consectetur adipiscing elit", OfferSalary = 149.50m, OfferDate = new DateTime(2024, 6, 20), RejectionReason = "Sample Item 2", JobPostingId = 6 }
                        );
                    }
                    if (!db.Onboardings.Any())
                    {
                        db.Onboardings.AddRange(
                    new Onboarding { Id = 9, StartDate = new DateTime(2024, 3, 15), ExpectedCompletionDate = new DateTime(2024, 3, 15), Status = (OnboardingStatus)0, Notes = "Lorem ipsum dolor sit amet", JobApplicationId = 7 },
                    new Onboarding { Id = 10, StartDate = new DateTime(2024, 6, 20), ExpectedCompletionDate = new DateTime(2024, 6, 20), Status = (OnboardingStatus)1, Notes = "Consectetur adipiscing elit", JobApplicationId = 8 }
                        );
                    }
                    if (!db.OnboardingTasks.Any())
                    {
                        db.OnboardingTasks.AddRange(
                    new OnboardingTask { Id = 29, Title = "Introduction to Physics", Description = "Lorem ipsum dolor sit amet", IsCompleted = true, CompletedDate = new DateTime(2024, 3, 15), AssignedTo = "Sample Item 1", DueDate = new DateTime(2024, 3, 15), OnboardingId = 9 },
                    new OnboardingTask { Id = 30, Title = "Advanced Mathematics", Description = "Consectetur adipiscing elit", IsCompleted = false, CompletedDate = new DateTime(2024, 6, 20), AssignedTo = "Sample Item 2", DueDate = new DateTime(2024, 6, 20), OnboardingId = 10 }
                        );
                    }
                    if (!db.SalaryRecords.Any())
                    {
                        db.SalaryRecords.AddRange(
                    new SalaryRecord { Id = 31, EffectiveDate = new DateTime(2024, 3, 15), GrossSalary = 99.99m, NetSalary = 99.99m, Currency = "Sample Ite", SalaryType = (SalaryRecordSalaryType)0, Notes = "Lorem ipsum dolor sit amet", EmployeeId = 11 },
                    new SalaryRecord { Id = 32, EffectiveDate = new DateTime(2024, 6, 20), GrossSalary = 149.50m, NetSalary = 149.50m, Currency = "Sample Ite", SalaryType = (SalaryRecordSalaryType)1, Notes = "Consectetur adipiscing elit", EmployeeId = 12 }
                        );
                    }
                    if (!db.SalaryDeductions.Any())
                    {
                        db.SalaryDeductions.AddRange(
                    new SalaryDeduction { Id = 33, DeductionType = "Sample Item 1", Amount = 99.99m, Currency = "Sample Ite", EffectiveDate = new DateTime(2024, 3, 15), Description = "Lorem ipsum dolor sit amet", SalaryRecordId = 31 },
                    new SalaryDeduction { Id = 34, DeductionType = "Sample Item 2", Amount = 149.50m, Currency = "Sample Ite", EffectiveDate = new DateTime(2024, 6, 20), Description = "Consectetur adipiscing elit", SalaryRecordId = 32 }
                        );
                    }
                    if (!db.TrainingPlans.Any())
                    {
                        db.TrainingPlans.AddRange(
                    new TrainingPlan { Id = 35, Title = "Introduction to Physics", Description = "Lorem ipsum dolor sit amet", Year = 42, Status = (TrainingPlanStatus)0, DepartmentId = 3 },
                    new TrainingPlan { Id = 36, Title = "Advanced Mathematics", Description = "Consectetur adipiscing elit", Year = 17, Status = (TrainingPlanStatus)1, DepartmentId = 4 }
                        );
                    }
                    if (!db.Trainings.Any())
                    {
                        db.Trainings.AddRange(
                    new Training { Id = 37, Title = "Introduction to Physics", Provider = "Sample Item 1", StartDate = new DateTime(2024, 3, 15), EndDate = new DateTime(2024, 3, 15), Location = "Sample Item 1", TrainingType = (TrainingTrainingType)0, Status = (TrainingStatus)0, Cost = 99.99m, Currency = "Sample Ite", TrainingPlanId = 35 },
                    new Training { Id = 38, Title = "Advanced Mathematics", Provider = "Sample Item 2", StartDate = new DateTime(2024, 6, 20), EndDate = new DateTime(2024, 6, 20), Location = "Sample Item 2", TrainingType = (TrainingTrainingType)1, Status = (TrainingStatus)1, Cost = 149.50m, Currency = "Sample Ite", TrainingPlanId = 36 }
                        );
                    }
                    if (!db.TrainingParticipations.Any())
                    {
                        db.TrainingParticipations.AddRange(
                    new TrainingParticipation { Id = 39, Attended = true, CompletionDate = new DateTime(2024, 3, 15), Score = 99.99m, CertificateEarned = true, Notes = "Lorem ipsum dolor sit amet", TrainingId = 37, EmployeeId = 11 },
                    new TrainingParticipation { Id = 40, Attended = false, CompletionDate = new DateTime(2024, 6, 20), Score = 149.50m, CertificateEarned = false, Notes = "Consectetur adipiscing elit", TrainingId = 38, EmployeeId = 12 }
                        );
                    }
                            db.SaveChanges();
                            Console.WriteLine("[Seed] Sample data created.");
                        }
                        catch (Exception sampleEx)
                        {
                            Console.WriteLine($"[Seed] Sample data skipped: {sampleEx.GetType().Name}: {sampleEx.Message}");
                            // Carry on — RBAC seed must still run so admin/123qwe is usable.
                        }
                        // Sync identity sequences to MAX(Id). Seeded rows carry explicit Ids which do NOT
                        // advance Postgres identity sequences → nextval collides with a seed row and the
                        // first few inserts fail with a duplicate-key 500. Runs every startup; idempotent.
                        try
                        {
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"Departments\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"Departments\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"Branches\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"Branches\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"Employees\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"Employees\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"EmployeeCertificates\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"EmployeeCertificates\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"DisciplinaryRecords\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"DisciplinaryRecords\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"OvertimeRecords\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"OvertimeRecords\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"LeaveTypes\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"LeaveTypes\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"LeaveRequests\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"LeaveRequests\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"PerformanceReviews\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"PerformanceReviews\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"PerformanceGoals\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"PerformanceGoals\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"PeerReviews\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"PeerReviews\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"JobPostings\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"JobPostings\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"JobApplications\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"JobApplications\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"Onboardings\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"Onboardings\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"OnboardingTasks\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"OnboardingTasks\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"SalaryRecords\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"SalaryRecords\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"SalaryDeductions\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"SalaryDeductions\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"TrainingPlans\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"TrainingPlans\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"Trainings\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"Trainings\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"TrainingParticipations\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"TrainingParticipations\") + 1, false);");
                            Console.WriteLine("[Seed] Identity sequences synced.");
                        }
                        catch (Exception seqEx)
                        {
                            Console.WriteLine($"[Seed] Sequence sync skipped: {seqEx.GetType().Name}: {seqEx.Message}");
                        }
                    }
                    // RBAC seed (Admin/User roles + permissions + admin user) runs through ABP DI
                    // so PermissionRegistry can be injected. SeedHelper is idempotent.
                    SeedHelper.SeedHostDb(Abp.Dependency.IocManager.Instance);
                    Console.WriteLine("[Seed] RBAC seed complete (Admin role + admin user).");
                }
                catch (Exception ex)
                {
                    // Full diagnostic — surface the real cause so silent seed failures are debuggable.
                    Console.WriteLine($"[Migration] FAILED: {ex.GetType().Name}: {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"[Migration] InnerException: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
                    Console.WriteLine("[Migration] StackTrace:");
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("[Migration] App continues without migration — admin user will not exist.");
                }
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

    public class Program
    {
        // Runtime entry: WebHost is required because ABP Startup returns IServiceProvider.
        public static void Main(string[] args)
        {
            // Npgsql 7+ requires UTC DateTimes — enable legacy behavior for ABP compatibility
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .Run();
        }

        // Design-time entry for EF Core tools (dotnet ef migrations).
        // Without this, EF tools wait 5 minutes for IHost build (resolver default timeout)
        // and then SIGTERM any running dotnet process — killing live dev servers.
        // We expose a minimal IHost that EF tools resolve in milliseconds; the actual
        // DbContext is built by IDesignTimeDbContextFactory in the EntityFrameworkCore project.
        public static IHostBuilder CreateHostBuilder(string[] args)
            => Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args);
    }
}
