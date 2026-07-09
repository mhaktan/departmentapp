using Abp.Authorization;
using Abp.Localization;

namespace DepartmentApp.Authorization
{
    public class DepartmentAppAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.GetPermissionOrNull("Pages") ?? context.CreatePermission("Pages", L("Pages"));

            // Department
            pages.CreateChildPermission(PermissionNames.Department_Read, L("Department.Read"));
            pages.CreateChildPermission(PermissionNames.Department_Create, L("Department.Create"));
            pages.CreateChildPermission(PermissionNames.Department_Update, L("Department.Update"));
            pages.CreateChildPermission(PermissionNames.Department_Delete, L("Department.Delete"));

            // Branch
            pages.CreateChildPermission(PermissionNames.Branch_Read, L("Branch.Read"));
            pages.CreateChildPermission(PermissionNames.Branch_Create, L("Branch.Create"));
            pages.CreateChildPermission(PermissionNames.Branch_Update, L("Branch.Update"));
            pages.CreateChildPermission(PermissionNames.Branch_Delete, L("Branch.Delete"));

            // Employee
            pages.CreateChildPermission(PermissionNames.Employee_Read, L("Employee.Read"));
            pages.CreateChildPermission(PermissionNames.Employee_Create, L("Employee.Create"));
            pages.CreateChildPermission(PermissionNames.Employee_Update, L("Employee.Update"));
            pages.CreateChildPermission(PermissionNames.Employee_Delete, L("Employee.Delete"));

            // EmployeeCertificate
            pages.CreateChildPermission(PermissionNames.EmployeeCertificate_Read, L("EmployeeCertificate.Read"));
            pages.CreateChildPermission(PermissionNames.EmployeeCertificate_Create, L("EmployeeCertificate.Create"));
            pages.CreateChildPermission(PermissionNames.EmployeeCertificate_Update, L("EmployeeCertificate.Update"));
            pages.CreateChildPermission(PermissionNames.EmployeeCertificate_Delete, L("EmployeeCertificate.Delete"));

            // DisciplinaryRecord
            pages.CreateChildPermission(PermissionNames.DisciplinaryRecord_Read, L("DisciplinaryRecord.Read"));
            pages.CreateChildPermission(PermissionNames.DisciplinaryRecord_Create, L("DisciplinaryRecord.Create"));
            pages.CreateChildPermission(PermissionNames.DisciplinaryRecord_Update, L("DisciplinaryRecord.Update"));
            pages.CreateChildPermission(PermissionNames.DisciplinaryRecord_Delete, L("DisciplinaryRecord.Delete"));
            pages.CreateChildPermission(PermissionNames.DisciplinaryRecord_ChangeStatus, L("DisciplinaryRecord.ChangeStatus"));

            // OvertimeRecord
            pages.CreateChildPermission(PermissionNames.OvertimeRecord_Read, L("OvertimeRecord.Read"));
            pages.CreateChildPermission(PermissionNames.OvertimeRecord_Create, L("OvertimeRecord.Create"));
            pages.CreateChildPermission(PermissionNames.OvertimeRecord_Update, L("OvertimeRecord.Update"));
            pages.CreateChildPermission(PermissionNames.OvertimeRecord_Delete, L("OvertimeRecord.Delete"));
            pages.CreateChildPermission(PermissionNames.OvertimeRecord_ChangeStatus, L("OvertimeRecord.ChangeStatus"));

            // LeaveType
            pages.CreateChildPermission(PermissionNames.LeaveType_Read, L("LeaveType.Read"));
            pages.CreateChildPermission(PermissionNames.LeaveType_Create, L("LeaveType.Create"));
            pages.CreateChildPermission(PermissionNames.LeaveType_Update, L("LeaveType.Update"));
            pages.CreateChildPermission(PermissionNames.LeaveType_Delete, L("LeaveType.Delete"));

            // LeaveRequest
            pages.CreateChildPermission(PermissionNames.LeaveRequest_Read, L("LeaveRequest.Read"));
            pages.CreateChildPermission(PermissionNames.LeaveRequest_Create, L("LeaveRequest.Create"));
            pages.CreateChildPermission(PermissionNames.LeaveRequest_Update, L("LeaveRequest.Update"));
            pages.CreateChildPermission(PermissionNames.LeaveRequest_Delete, L("LeaveRequest.Delete"));
            pages.CreateChildPermission(PermissionNames.LeaveRequest_ChangeStatus, L("LeaveRequest.ChangeStatus"));

            // PerformanceReview
            pages.CreateChildPermission(PermissionNames.PerformanceReview_Read, L("PerformanceReview.Read"));
            pages.CreateChildPermission(PermissionNames.PerformanceReview_Create, L("PerformanceReview.Create"));
            pages.CreateChildPermission(PermissionNames.PerformanceReview_Update, L("PerformanceReview.Update"));
            pages.CreateChildPermission(PermissionNames.PerformanceReview_Delete, L("PerformanceReview.Delete"));
            pages.CreateChildPermission(PermissionNames.PerformanceReview_ChangeStatus, L("PerformanceReview.ChangeStatus"));

            // PerformanceGoal
            pages.CreateChildPermission(PermissionNames.PerformanceGoal_Read, L("PerformanceGoal.Read"));
            pages.CreateChildPermission(PermissionNames.PerformanceGoal_Create, L("PerformanceGoal.Create"));
            pages.CreateChildPermission(PermissionNames.PerformanceGoal_Update, L("PerformanceGoal.Update"));
            pages.CreateChildPermission(PermissionNames.PerformanceGoal_Delete, L("PerformanceGoal.Delete"));

            // PeerReview
            pages.CreateChildPermission(PermissionNames.PeerReview_Read, L("PeerReview.Read"));
            pages.CreateChildPermission(PermissionNames.PeerReview_Create, L("PeerReview.Create"));
            pages.CreateChildPermission(PermissionNames.PeerReview_Update, L("PeerReview.Update"));
            pages.CreateChildPermission(PermissionNames.PeerReview_Delete, L("PeerReview.Delete"));

            // JobPosting
            pages.CreateChildPermission(PermissionNames.JobPosting_Read, L("JobPosting.Read"));
            pages.CreateChildPermission(PermissionNames.JobPosting_Create, L("JobPosting.Create"));
            pages.CreateChildPermission(PermissionNames.JobPosting_Update, L("JobPosting.Update"));
            pages.CreateChildPermission(PermissionNames.JobPosting_Delete, L("JobPosting.Delete"));
            pages.CreateChildPermission(PermissionNames.JobPosting_ChangeStatus, L("JobPosting.ChangeStatus"));

            // JobApplication
            pages.CreateChildPermission(PermissionNames.JobApplication_Read, L("JobApplication.Read"));
            pages.CreateChildPermission(PermissionNames.JobApplication_Create, L("JobApplication.Create"));
            pages.CreateChildPermission(PermissionNames.JobApplication_Update, L("JobApplication.Update"));
            pages.CreateChildPermission(PermissionNames.JobApplication_Delete, L("JobApplication.Delete"));
            pages.CreateChildPermission(PermissionNames.JobApplication_ChangeStatus, L("JobApplication.ChangeStatus"));

            // Onboarding
            pages.CreateChildPermission(PermissionNames.Onboarding_Read, L("Onboarding.Read"));
            pages.CreateChildPermission(PermissionNames.Onboarding_Create, L("Onboarding.Create"));
            pages.CreateChildPermission(PermissionNames.Onboarding_Update, L("Onboarding.Update"));
            pages.CreateChildPermission(PermissionNames.Onboarding_Delete, L("Onboarding.Delete"));
            pages.CreateChildPermission(PermissionNames.Onboarding_ChangeStatus, L("Onboarding.ChangeStatus"));

            // OnboardingTask
            pages.CreateChildPermission(PermissionNames.OnboardingTask_Read, L("OnboardingTask.Read"));
            pages.CreateChildPermission(PermissionNames.OnboardingTask_Create, L("OnboardingTask.Create"));
            pages.CreateChildPermission(PermissionNames.OnboardingTask_Update, L("OnboardingTask.Update"));
            pages.CreateChildPermission(PermissionNames.OnboardingTask_Delete, L("OnboardingTask.Delete"));

            // SalaryRecord
            pages.CreateChildPermission(PermissionNames.SalaryRecord_Read, L("SalaryRecord.Read"));
            pages.CreateChildPermission(PermissionNames.SalaryRecord_Create, L("SalaryRecord.Create"));
            pages.CreateChildPermission(PermissionNames.SalaryRecord_Update, L("SalaryRecord.Update"));
            pages.CreateChildPermission(PermissionNames.SalaryRecord_Delete, L("SalaryRecord.Delete"));

            // SalaryDeduction
            pages.CreateChildPermission(PermissionNames.SalaryDeduction_Read, L("SalaryDeduction.Read"));
            pages.CreateChildPermission(PermissionNames.SalaryDeduction_Create, L("SalaryDeduction.Create"));
            pages.CreateChildPermission(PermissionNames.SalaryDeduction_Update, L("SalaryDeduction.Update"));
            pages.CreateChildPermission(PermissionNames.SalaryDeduction_Delete, L("SalaryDeduction.Delete"));

            // TrainingPlan
            pages.CreateChildPermission(PermissionNames.TrainingPlan_Read, L("TrainingPlan.Read"));
            pages.CreateChildPermission(PermissionNames.TrainingPlan_Create, L("TrainingPlan.Create"));
            pages.CreateChildPermission(PermissionNames.TrainingPlan_Update, L("TrainingPlan.Update"));
            pages.CreateChildPermission(PermissionNames.TrainingPlan_Delete, L("TrainingPlan.Delete"));
            pages.CreateChildPermission(PermissionNames.TrainingPlan_ChangeStatus, L("TrainingPlan.ChangeStatus"));

            // Training
            pages.CreateChildPermission(PermissionNames.Training_Read, L("Training.Read"));
            pages.CreateChildPermission(PermissionNames.Training_Create, L("Training.Create"));
            pages.CreateChildPermission(PermissionNames.Training_Update, L("Training.Update"));
            pages.CreateChildPermission(PermissionNames.Training_Delete, L("Training.Delete"));
            pages.CreateChildPermission(PermissionNames.Training_ChangeStatus, L("Training.ChangeStatus"));

            // TrainingParticipation
            pages.CreateChildPermission(PermissionNames.TrainingParticipation_Read, L("TrainingParticipation.Read"));
            pages.CreateChildPermission(PermissionNames.TrainingParticipation_Create, L("TrainingParticipation.Create"));
            pages.CreateChildPermission(PermissionNames.TrainingParticipation_Update, L("TrainingParticipation.Update"));
            pages.CreateChildPermission(PermissionNames.TrainingParticipation_Delete, L("TrainingParticipation.Delete"));

            // RBAC
            pages.CreateChildPermission(PermissionNames.AppUser_Read, L("AppUser.Read"));
            pages.CreateChildPermission(PermissionNames.AppRole_Read, L("AppRole.Read"));
            pages.CreateChildPermission(PermissionNames.AppUser_Create, L("AppUser.Create"));
            pages.CreateChildPermission(PermissionNames.AppRole_Create, L("AppRole.Create"));
            pages.CreateChildPermission(PermissionNames.AppUser_Update, L("AppUser.Update"));
            pages.CreateChildPermission(PermissionNames.AppRole_Update, L("AppRole.Update"));
            pages.CreateChildPermission(PermissionNames.AppUser_Delete, L("AppUser.Delete"));
            pages.CreateChildPermission(PermissionNames.AppRole_Delete, L("AppRole.Delete"));
            pages.CreateChildPermission(PermissionNames.AppRole_AssignPermissions, L("AppRole.AssignPermissions"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, DepartmentAppConsts.LocalizationSourceName);
        }
    }
}
