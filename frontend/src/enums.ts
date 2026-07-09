// ---------------------------------------------------------------------------
// Enum definitions — auto-generated from ER model
// Maps integer values to display labels for enum fields
// ---------------------------------------------------------------------------

// Employee.gender
export const EmployeeGenderMap: Record<string, string> = {
  '0': 'Male',
  '1': 'Female',
  '2': 'Other'
};
export const EmployeeGenderOptions = [
  { label: 'Male', value: '0' },
  { label: 'Female', value: '1' },
  { label: 'Other', value: '2' }
];

// Employee.employmentType
export const EmployeeEmploymentTypeMap: Record<string, string> = {
  '0': 'FullTime',
  '1': 'PartTime',
  '2': 'Contract',
  '3': 'Intern'
};
export const EmployeeEmploymentTypeOptions = [
  { label: 'FullTime', value: '0' },
  { label: 'PartTime', value: '1' },
  { label: 'Contract', value: '2' },
  { label: 'Intern', value: '3' }
];

// Employee.status
export const EmployeeStatusMap: Record<string, string> = {
  '0': 'Active',
  '1': 'OnLeave',
  '2': 'Terminated',
  '3': 'Suspended'
};
export const EmployeeStatusOptions = [
  { label: 'Active', value: '0' },
  { label: 'OnLeave', value: '1' },
  { label: 'Terminated', value: '2' },
  { label: 'Suspended', value: '3' }
];

// DisciplinaryRecord.type
export const DisciplinaryRecordTypeMap: Record<string, string> = {
  '0': 'VerbalWarning',
  '1': 'WrittenWarning',
  '2': 'FinalWarning',
  '3': 'Suspension',
  '4': 'Termination'
};
export const DisciplinaryRecordTypeOptions = [
  { label: 'VerbalWarning', value: '0' },
  { label: 'WrittenWarning', value: '1' },
  { label: 'FinalWarning', value: '2' },
  { label: 'Suspension', value: '3' },
  { label: 'Termination', value: '4' }
];

// DisciplinaryRecord.status
export const DisciplinaryRecordStatusMap: Record<string, string> = {
  '0': 'Open',
  '1': 'UnderReview',
  '2': 'Appealed',
  '3': 'Resolved',
  '4': 'Closed'
};
export const DisciplinaryRecordStatusOptions = [
  { label: 'Open', value: '0' },
  { label: 'UnderReview', value: '1' },
  { label: 'Appealed', value: '2' },
  { label: 'Resolved', value: '3' },
  { label: 'Closed', value: '4' }
];

// OvertimeRecord.status
export const OvertimeRecordStatusMap: Record<string, string> = {
  '0': 'Pending',
  '1': 'Approved',
  '2': 'Rejected',
  '3': 'Cancelled'
};
export const OvertimeRecordStatusOptions = [
  { label: 'Pending', value: '0' },
  { label: 'Approved', value: '1' },
  { label: 'Rejected', value: '2' },
  { label: 'Cancelled', value: '3' }
];

// LeaveRequest.status
export const LeaveRequestStatusMap: Record<string, string> = {
  '0': 'Draft',
  '1': 'PendingManagerApproval',
  '2': 'PendingHRApproval',
  '3': 'Approved',
  '4': 'Revision',
  '5': 'Cancelled',
  '6': 'Rejected'
};
export const LeaveRequestStatusOptions = [
  { label: 'Draft', value: '0' },
  { label: 'PendingManagerApproval', value: '1' },
  { label: 'PendingHRApproval', value: '2' },
  { label: 'Approved', value: '3' },
  { label: 'Revision', value: '4' },
  { label: 'Cancelled', value: '5' },
  { label: 'Rejected', value: '6' }
];

// PerformanceReview.reviewType
export const PerformanceReviewReviewTypeMap: Record<string, string> = {
  '0': 'Annual',
  '1': 'MidYear',
  '2': 'Probation'
};
export const PerformanceReviewReviewTypeOptions = [
  { label: 'Annual', value: '0' },
  { label: 'MidYear', value: '1' },
  { label: 'Probation', value: '2' }
];

// PerformanceReview.status
export const PerformanceReviewStatusMap: Record<string, string> = {
  '0': 'Draft',
  '1': 'SelfAssessmentPending',
  '2': 'ManagerReviewPending',
  '3': 'PeerReviewPending',
  '4': 'HRReviewPending',
  '5': 'Completed',
  '6': 'Cancelled'
};
export const PerformanceReviewStatusOptions = [
  { label: 'Draft', value: '0' },
  { label: 'SelfAssessmentPending', value: '1' },
  { label: 'ManagerReviewPending', value: '2' },
  { label: 'PeerReviewPending', value: '3' },
  { label: 'HRReviewPending', value: '4' },
  { label: 'Completed', value: '5' },
  { label: 'Cancelled', value: '6' }
];

// PerformanceGoal.status
export const PerformanceGoalStatusMap: Record<string, string> = {
  '0': 'Active',
  '1': 'Completed',
  '2': 'Cancelled'
};
export const PerformanceGoalStatusOptions = [
  { label: 'Active', value: '0' },
  { label: 'Completed', value: '1' },
  { label: 'Cancelled', value: '2' }
];

// JobPosting.status
export const JobPostingStatusMap: Record<string, string> = {
  '0': 'Draft',
  '1': 'Published',
  '2': 'Closed',
  '3': 'Cancelled'
};
export const JobPostingStatusOptions = [
  { label: 'Draft', value: '0' },
  { label: 'Published', value: '1' },
  { label: 'Closed', value: '2' },
  { label: 'Cancelled', value: '3' }
];

// JobPosting.employmentType
export const JobPostingEmploymentTypeMap: Record<string, string> = {
  '0': 'FullTime',
  '1': 'PartTime',
  '2': 'Contract',
  '3': 'Intern'
};
export const JobPostingEmploymentTypeOptions = [
  { label: 'FullTime', value: '0' },
  { label: 'PartTime', value: '1' },
  { label: 'Contract', value: '2' },
  { label: 'Intern', value: '3' }
];

// JobApplication.status
export const JobApplicationStatusMap: Record<string, string> = {
  '0': 'Received',
  '1': 'Screening',
  '2': 'Interview',
  '3': 'OfferPending',
  '4': 'OfferAccepted',
  '5': 'OfferRejected',
  '6': 'Rejected'
};
export const JobApplicationStatusOptions = [
  { label: 'Received', value: '0' },
  { label: 'Screening', value: '1' },
  { label: 'Interview', value: '2' },
  { label: 'OfferPending', value: '3' },
  { label: 'OfferAccepted', value: '4' },
  { label: 'OfferRejected', value: '5' },
  { label: 'Rejected', value: '6' }
];

// Onboarding.status
export const OnboardingStatusMap: Record<string, string> = {
  '0': 'NotStarted',
  '1': 'InProgress',
  '2': 'Completed',
  '3': 'Cancelled'
};
export const OnboardingStatusOptions = [
  { label: 'NotStarted', value: '0' },
  { label: 'InProgress', value: '1' },
  { label: 'Completed', value: '2' },
  { label: 'Cancelled', value: '3' }
];

// SalaryRecord.salaryType
export const SalaryRecordSalaryTypeMap: Record<string, string> = {
  '0': 'Monthly',
  '1': 'Hourly',
  '2': 'Daily'
};
export const SalaryRecordSalaryTypeOptions = [
  { label: 'Monthly', value: '0' },
  { label: 'Hourly', value: '1' },
  { label: 'Daily', value: '2' }
];

// TrainingPlan.status
export const TrainingPlanStatusMap: Record<string, string> = {
  '0': 'Draft',
  '1': 'Active',
  '2': 'Completed',
  '3': 'Cancelled'
};
export const TrainingPlanStatusOptions = [
  { label: 'Draft', value: '0' },
  { label: 'Active', value: '1' },
  { label: 'Completed', value: '2' },
  { label: 'Cancelled', value: '3' }
];

// Training.trainingType
export const TrainingTrainingTypeMap: Record<string, string> = {
  '0': 'Internal',
  '1': 'External',
  '2': 'Online',
  '3': 'OnTheJob'
};
export const TrainingTrainingTypeOptions = [
  { label: 'Internal', value: '0' },
  { label: 'External', value: '1' },
  { label: 'Online', value: '2' },
  { label: 'OnTheJob', value: '3' }
];

// Training.status
export const TrainingStatusMap: Record<string, string> = {
  '0': 'Planned',
  '1': 'Ongoing',
  '2': 'Completed',
  '3': 'Cancelled'
};
export const TrainingStatusOptions = [
  { label: 'Planned', value: '0' },
  { label: 'Ongoing', value: '1' },
  { label: 'Completed', value: '2' },
  { label: 'Cancelled', value: '3' }
];
