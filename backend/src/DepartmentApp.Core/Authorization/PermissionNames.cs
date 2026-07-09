namespace DepartmentApp.Authorization
{
    public static class PermissionNames
    {
        public const string Pages = "Pages";

        // Department
        public const string Department_Read = "Department.Read";
        public const string Department_Create = "Department.Create";
        public const string Department_Update = "Department.Update";
        public const string Department_Delete = "Department.Delete";

        // Branch
        public const string Branch_Read = "Branch.Read";
        public const string Branch_Create = "Branch.Create";
        public const string Branch_Update = "Branch.Update";
        public const string Branch_Delete = "Branch.Delete";

        // Employee
        public const string Employee_Read = "Employee.Read";
        public const string Employee_Create = "Employee.Create";
        public const string Employee_Update = "Employee.Update";
        public const string Employee_Delete = "Employee.Delete";

        // EmployeeCertificate
        public const string EmployeeCertificate_Read = "EmployeeCertificate.Read";
        public const string EmployeeCertificate_Create = "EmployeeCertificate.Create";
        public const string EmployeeCertificate_Update = "EmployeeCertificate.Update";
        public const string EmployeeCertificate_Delete = "EmployeeCertificate.Delete";

        // DisciplinaryRecord
        public const string DisciplinaryRecord_Read = "DisciplinaryRecord.Read";
        public const string DisciplinaryRecord_Create = "DisciplinaryRecord.Create";
        public const string DisciplinaryRecord_Update = "DisciplinaryRecord.Update";
        public const string DisciplinaryRecord_Delete = "DisciplinaryRecord.Delete";
        public const string DisciplinaryRecord_ChangeStatus = "DisciplinaryRecord.ChangeStatus";

        // OvertimeRecord
        public const string OvertimeRecord_Read = "OvertimeRecord.Read";
        public const string OvertimeRecord_Create = "OvertimeRecord.Create";
        public const string OvertimeRecord_Update = "OvertimeRecord.Update";
        public const string OvertimeRecord_Delete = "OvertimeRecord.Delete";
        public const string OvertimeRecord_ChangeStatus = "OvertimeRecord.ChangeStatus";

        // LeaveType
        public const string LeaveType_Read = "LeaveType.Read";
        public const string LeaveType_Create = "LeaveType.Create";
        public const string LeaveType_Update = "LeaveType.Update";
        public const string LeaveType_Delete = "LeaveType.Delete";

        // LeaveRequest
        public const string LeaveRequest_Read = "LeaveRequest.Read";
        public const string LeaveRequest_Create = "LeaveRequest.Create";
        public const string LeaveRequest_Update = "LeaveRequest.Update";
        public const string LeaveRequest_Delete = "LeaveRequest.Delete";
        public const string LeaveRequest_ChangeStatus = "LeaveRequest.ChangeStatus";

        // PerformanceReview
        public const string PerformanceReview_Read = "PerformanceReview.Read";
        public const string PerformanceReview_Create = "PerformanceReview.Create";
        public const string PerformanceReview_Update = "PerformanceReview.Update";
        public const string PerformanceReview_Delete = "PerformanceReview.Delete";
        public const string PerformanceReview_ChangeStatus = "PerformanceReview.ChangeStatus";

        // PerformanceGoal
        public const string PerformanceGoal_Read = "PerformanceGoal.Read";
        public const string PerformanceGoal_Create = "PerformanceGoal.Create";
        public const string PerformanceGoal_Update = "PerformanceGoal.Update";
        public const string PerformanceGoal_Delete = "PerformanceGoal.Delete";

        // PeerReview
        public const string PeerReview_Read = "PeerReview.Read";
        public const string PeerReview_Create = "PeerReview.Create";
        public const string PeerReview_Update = "PeerReview.Update";
        public const string PeerReview_Delete = "PeerReview.Delete";

        // JobPosting
        public const string JobPosting_Read = "JobPosting.Read";
        public const string JobPosting_Create = "JobPosting.Create";
        public const string JobPosting_Update = "JobPosting.Update";
        public const string JobPosting_Delete = "JobPosting.Delete";
        public const string JobPosting_ChangeStatus = "JobPosting.ChangeStatus";

        // JobApplication
        public const string JobApplication_Read = "JobApplication.Read";
        public const string JobApplication_Create = "JobApplication.Create";
        public const string JobApplication_Update = "JobApplication.Update";
        public const string JobApplication_Delete = "JobApplication.Delete";
        public const string JobApplication_ChangeStatus = "JobApplication.ChangeStatus";

        // Onboarding
        public const string Onboarding_Read = "Onboarding.Read";
        public const string Onboarding_Create = "Onboarding.Create";
        public const string Onboarding_Update = "Onboarding.Update";
        public const string Onboarding_Delete = "Onboarding.Delete";
        public const string Onboarding_ChangeStatus = "Onboarding.ChangeStatus";

        // OnboardingTask
        public const string OnboardingTask_Read = "OnboardingTask.Read";
        public const string OnboardingTask_Create = "OnboardingTask.Create";
        public const string OnboardingTask_Update = "OnboardingTask.Update";
        public const string OnboardingTask_Delete = "OnboardingTask.Delete";

        // SalaryRecord
        public const string SalaryRecord_Read = "SalaryRecord.Read";
        public const string SalaryRecord_Create = "SalaryRecord.Create";
        public const string SalaryRecord_Update = "SalaryRecord.Update";
        public const string SalaryRecord_Delete = "SalaryRecord.Delete";

        // SalaryDeduction
        public const string SalaryDeduction_Read = "SalaryDeduction.Read";
        public const string SalaryDeduction_Create = "SalaryDeduction.Create";
        public const string SalaryDeduction_Update = "SalaryDeduction.Update";
        public const string SalaryDeduction_Delete = "SalaryDeduction.Delete";

        // TrainingPlan
        public const string TrainingPlan_Read = "TrainingPlan.Read";
        public const string TrainingPlan_Create = "TrainingPlan.Create";
        public const string TrainingPlan_Update = "TrainingPlan.Update";
        public const string TrainingPlan_Delete = "TrainingPlan.Delete";
        public const string TrainingPlan_ChangeStatus = "TrainingPlan.ChangeStatus";

        // Training
        public const string Training_Read = "Training.Read";
        public const string Training_Create = "Training.Create";
        public const string Training_Update = "Training.Update";
        public const string Training_Delete = "Training.Delete";
        public const string Training_ChangeStatus = "Training.ChangeStatus";

        // TrainingParticipation
        public const string TrainingParticipation_Read = "TrainingParticipation.Read";
        public const string TrainingParticipation_Create = "TrainingParticipation.Create";
        public const string TrainingParticipation_Update = "TrainingParticipation.Update";
        public const string TrainingParticipation_Delete = "TrainingParticipation.Delete";

        // RBAC management
        public const string AppUser_Read = "AppUser.Read";
        public const string AppRole_Read = "AppRole.Read";
        public const string AppUser_Create = "AppUser.Create";
        public const string AppRole_Create = "AppRole.Create";
        public const string AppUser_Update = "AppUser.Update";
        public const string AppRole_Update = "AppRole.Update";
        public const string AppUser_Delete = "AppUser.Delete";
        public const string AppRole_Delete = "AppRole.Delete";
        public const string AppRole_AssignPermissions = "AppRole.AssignPermissions";

    }
}
