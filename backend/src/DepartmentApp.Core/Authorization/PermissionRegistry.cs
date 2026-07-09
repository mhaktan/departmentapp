using System.Collections.Generic;
using Abp.Dependency;

namespace DepartmentApp.Authorization
{
    /// <summary>Single permission descriptor — name, group (entity), description.</summary>
    public class PermissionInfo
    {
        public string Name { get; }
        public string Group { get; }
        public string Description { get; }
        public bool IsRbac { get; }

        public PermissionInfo(string name, string group, string description, bool isRbac)
        {
            Name = name; Group = group; Description = description; IsRbac = isRbac;
        }
    }

    public interface IPermissionRegistry
    {
        IReadOnlyList<PermissionInfo> All { get; }
    }

    public class PermissionRegistry : IPermissionRegistry, ISingletonDependency
    {
        public IReadOnlyList<PermissionInfo> All { get; } = new List<PermissionInfo>
        {
            new PermissionInfo("Department.Read", "Department", "Read Department", false),
            new PermissionInfo("Department.Create", "Department", "Create Department", false),
            new PermissionInfo("Department.Update", "Department", "Update Department", false),
            new PermissionInfo("Department.Delete", "Department", "Delete Department", false),
            new PermissionInfo("Branch.Read", "Branch", "Read Branch", false),
            new PermissionInfo("Branch.Create", "Branch", "Create Branch", false),
            new PermissionInfo("Branch.Update", "Branch", "Update Branch", false),
            new PermissionInfo("Branch.Delete", "Branch", "Delete Branch", false),
            new PermissionInfo("Employee.Read", "Employee", "Read Employee", false),
            new PermissionInfo("Employee.Create", "Employee", "Create Employee", false),
            new PermissionInfo("Employee.Update", "Employee", "Update Employee", false),
            new PermissionInfo("Employee.Delete", "Employee", "Delete Employee", false),
            new PermissionInfo("EmployeeCertificate.Read", "EmployeeCertificate", "Read EmployeeCertificate", false),
            new PermissionInfo("EmployeeCertificate.Create", "EmployeeCertificate", "Create EmployeeCertificate", false),
            new PermissionInfo("EmployeeCertificate.Update", "EmployeeCertificate", "Update EmployeeCertificate", false),
            new PermissionInfo("EmployeeCertificate.Delete", "EmployeeCertificate", "Delete EmployeeCertificate", false),
            new PermissionInfo("DisciplinaryRecord.Read", "DisciplinaryRecord", "Read DisciplinaryRecord", false),
            new PermissionInfo("DisciplinaryRecord.Create", "DisciplinaryRecord", "Create DisciplinaryRecord", false),
            new PermissionInfo("DisciplinaryRecord.Update", "DisciplinaryRecord", "Update DisciplinaryRecord", false),
            new PermissionInfo("DisciplinaryRecord.Delete", "DisciplinaryRecord", "Delete DisciplinaryRecord", false),
            new PermissionInfo("DisciplinaryRecord.ChangeStatus", "DisciplinaryRecord", "Change DisciplinaryRecord status", false),
            new PermissionInfo("OvertimeRecord.Read", "OvertimeRecord", "Read OvertimeRecord", false),
            new PermissionInfo("OvertimeRecord.Create", "OvertimeRecord", "Create OvertimeRecord", false),
            new PermissionInfo("OvertimeRecord.Update", "OvertimeRecord", "Update OvertimeRecord", false),
            new PermissionInfo("OvertimeRecord.Delete", "OvertimeRecord", "Delete OvertimeRecord", false),
            new PermissionInfo("OvertimeRecord.ChangeStatus", "OvertimeRecord", "Change OvertimeRecord status", false),
            new PermissionInfo("LeaveType.Read", "LeaveType", "Read LeaveType", false),
            new PermissionInfo("LeaveType.Create", "LeaveType", "Create LeaveType", false),
            new PermissionInfo("LeaveType.Update", "LeaveType", "Update LeaveType", false),
            new PermissionInfo("LeaveType.Delete", "LeaveType", "Delete LeaveType", false),
            new PermissionInfo("LeaveRequest.Read", "LeaveRequest", "Read LeaveRequest", false),
            new PermissionInfo("LeaveRequest.Create", "LeaveRequest", "Create LeaveRequest", false),
            new PermissionInfo("LeaveRequest.Update", "LeaveRequest", "Update LeaveRequest", false),
            new PermissionInfo("LeaveRequest.Delete", "LeaveRequest", "Delete LeaveRequest", false),
            new PermissionInfo("LeaveRequest.ChangeStatus", "LeaveRequest", "Change LeaveRequest status", false),
            new PermissionInfo("PerformanceReview.Read", "PerformanceReview", "Read PerformanceReview", false),
            new PermissionInfo("PerformanceReview.Create", "PerformanceReview", "Create PerformanceReview", false),
            new PermissionInfo("PerformanceReview.Update", "PerformanceReview", "Update PerformanceReview", false),
            new PermissionInfo("PerformanceReview.Delete", "PerformanceReview", "Delete PerformanceReview", false),
            new PermissionInfo("PerformanceReview.ChangeStatus", "PerformanceReview", "Change PerformanceReview status", false),
            new PermissionInfo("PerformanceGoal.Read", "PerformanceGoal", "Read PerformanceGoal", false),
            new PermissionInfo("PerformanceGoal.Create", "PerformanceGoal", "Create PerformanceGoal", false),
            new PermissionInfo("PerformanceGoal.Update", "PerformanceGoal", "Update PerformanceGoal", false),
            new PermissionInfo("PerformanceGoal.Delete", "PerformanceGoal", "Delete PerformanceGoal", false),
            new PermissionInfo("PeerReview.Read", "PeerReview", "Read PeerReview", false),
            new PermissionInfo("PeerReview.Create", "PeerReview", "Create PeerReview", false),
            new PermissionInfo("PeerReview.Update", "PeerReview", "Update PeerReview", false),
            new PermissionInfo("PeerReview.Delete", "PeerReview", "Delete PeerReview", false),
            new PermissionInfo("JobPosting.Read", "JobPosting", "Read JobPosting", false),
            new PermissionInfo("JobPosting.Create", "JobPosting", "Create JobPosting", false),
            new PermissionInfo("JobPosting.Update", "JobPosting", "Update JobPosting", false),
            new PermissionInfo("JobPosting.Delete", "JobPosting", "Delete JobPosting", false),
            new PermissionInfo("JobPosting.ChangeStatus", "JobPosting", "Change JobPosting status", false),
            new PermissionInfo("JobApplication.Read", "JobApplication", "Read JobApplication", false),
            new PermissionInfo("JobApplication.Create", "JobApplication", "Create JobApplication", false),
            new PermissionInfo("JobApplication.Update", "JobApplication", "Update JobApplication", false),
            new PermissionInfo("JobApplication.Delete", "JobApplication", "Delete JobApplication", false),
            new PermissionInfo("JobApplication.ChangeStatus", "JobApplication", "Change JobApplication status", false),
            new PermissionInfo("Onboarding.Read", "Onboarding", "Read Onboarding", false),
            new PermissionInfo("Onboarding.Create", "Onboarding", "Create Onboarding", false),
            new PermissionInfo("Onboarding.Update", "Onboarding", "Update Onboarding", false),
            new PermissionInfo("Onboarding.Delete", "Onboarding", "Delete Onboarding", false),
            new PermissionInfo("Onboarding.ChangeStatus", "Onboarding", "Change Onboarding status", false),
            new PermissionInfo("OnboardingTask.Read", "OnboardingTask", "Read OnboardingTask", false),
            new PermissionInfo("OnboardingTask.Create", "OnboardingTask", "Create OnboardingTask", false),
            new PermissionInfo("OnboardingTask.Update", "OnboardingTask", "Update OnboardingTask", false),
            new PermissionInfo("OnboardingTask.Delete", "OnboardingTask", "Delete OnboardingTask", false),
            new PermissionInfo("SalaryRecord.Read", "SalaryRecord", "Read SalaryRecord", false),
            new PermissionInfo("SalaryRecord.Create", "SalaryRecord", "Create SalaryRecord", false),
            new PermissionInfo("SalaryRecord.Update", "SalaryRecord", "Update SalaryRecord", false),
            new PermissionInfo("SalaryRecord.Delete", "SalaryRecord", "Delete SalaryRecord", false),
            new PermissionInfo("SalaryDeduction.Read", "SalaryDeduction", "Read SalaryDeduction", false),
            new PermissionInfo("SalaryDeduction.Create", "SalaryDeduction", "Create SalaryDeduction", false),
            new PermissionInfo("SalaryDeduction.Update", "SalaryDeduction", "Update SalaryDeduction", false),
            new PermissionInfo("SalaryDeduction.Delete", "SalaryDeduction", "Delete SalaryDeduction", false),
            new PermissionInfo("TrainingPlan.Read", "TrainingPlan", "Read TrainingPlan", false),
            new PermissionInfo("TrainingPlan.Create", "TrainingPlan", "Create TrainingPlan", false),
            new PermissionInfo("TrainingPlan.Update", "TrainingPlan", "Update TrainingPlan", false),
            new PermissionInfo("TrainingPlan.Delete", "TrainingPlan", "Delete TrainingPlan", false),
            new PermissionInfo("TrainingPlan.ChangeStatus", "TrainingPlan", "Change TrainingPlan status", false),
            new PermissionInfo("Training.Read", "Training", "Read Training", false),
            new PermissionInfo("Training.Create", "Training", "Create Training", false),
            new PermissionInfo("Training.Update", "Training", "Update Training", false),
            new PermissionInfo("Training.Delete", "Training", "Delete Training", false),
            new PermissionInfo("Training.ChangeStatus", "Training", "Change Training status", false),
            new PermissionInfo("TrainingParticipation.Read", "TrainingParticipation", "Read TrainingParticipation", false),
            new PermissionInfo("TrainingParticipation.Create", "TrainingParticipation", "Create TrainingParticipation", false),
            new PermissionInfo("TrainingParticipation.Update", "TrainingParticipation", "Update TrainingParticipation", false),
            new PermissionInfo("TrainingParticipation.Delete", "TrainingParticipation", "Delete TrainingParticipation", false),
            new PermissionInfo("AppUser.Read", "AppUser", "Read users", true),
            new PermissionInfo("AppRole.Read", "AppRole", "Read roles", true),
            new PermissionInfo("AppUser.Create", "AppUser", "Create users", true),
            new PermissionInfo("AppRole.Create", "AppRole", "Create roles", true),
            new PermissionInfo("AppUser.Update", "AppUser", "Update users", true),
            new PermissionInfo("AppRole.Update", "AppRole", "Update roles", true),
            new PermissionInfo("AppUser.Delete", "AppUser", "Delete users", true),
            new PermissionInfo("AppRole.Delete", "AppRole", "Delete roles", true),
            new PermissionInfo("AppRole.AssignPermissions", "AppRole", "Assign permissions to roles", true),
        };
    }
}
