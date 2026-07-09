import React, { useState } from 'react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { BrowserRouter, Route, Routes, Navigate } from 'react-router-dom';
import { isAuthenticated, clearAuth } from './dataProvider';
import { LoginPage } from './screens/LoginPage';
import { GlobalMenu } from './components/GlobalMenu';
import { FlowProvider } from './flows/FlowProvider';
import { DepartmentList } from './screens/Department/DepartmentList';
import { BranchList } from './screens/Branch/BranchList';
import { EmployeeList } from './screens/Employee/EmployeeList';
import { EmployeeCertificateList } from './screens/EmployeeCertificate/EmployeeCertificateList';
import { DisciplinaryRecordList } from './screens/DisciplinaryRecord/DisciplinaryRecordList';
import { OvertimeRecordList } from './screens/OvertimeRecord/OvertimeRecordList';
import { LeaveTypeList } from './screens/LeaveType/LeaveTypeList';
import { LeaveRequestList } from './screens/LeaveRequest/LeaveRequestList';
import { PerformanceReviewList } from './screens/PerformanceReview/PerformanceReviewList';
import { PerformanceGoalList } from './screens/PerformanceGoal/PerformanceGoalList';
import { PeerReviewList } from './screens/PeerReview/PeerReviewList';
import { JobPostingList } from './screens/JobPosting/JobPostingList';
import { JobApplicationList } from './screens/JobApplication/JobApplicationList';
import { OnboardingList } from './screens/Onboarding/OnboardingList';
import { OnboardingTaskList } from './screens/OnboardingTask/OnboardingTaskList';
import { SalaryRecordList } from './screens/SalaryRecord/SalaryRecordList';
import { SalaryDeductionList } from './screens/SalaryDeduction/SalaryDeductionList';
import { TrainingPlanList } from './screens/TrainingPlan/TrainingPlanList';
import { TrainingList } from './screens/Training/TrainingList';
import { TrainingParticipationList } from './screens/TrainingParticipation/TrainingParticipationList';
import { DashboardScreen } from './screens/dashboard/DashboardScreen';
import TaskInboxScreen from './screens/tasks/TaskInboxScreen';
import UserListScreen from './admin/UserListScreen';
import RoleListScreen from './admin/RoleListScreen';

const queryClient = new QueryClient({
  defaultOptions: { queries: { retry: 1, staleTime: 0 } },
});

export const App: React.FC = () => {
  const [authenticated, setAuthenticated] = useState(isAuthenticated());


  const handleLogout = () => {
    clearAuth();
    setAuthenticated(false);
    queryClient.clear();
  };

  return (
    <QueryClientProvider client={queryClient}>
      <FlowProvider>
      <BrowserRouter>
        <Routes>
          <Route path="*" element={
            authenticated
              ? (
                <GlobalMenu>
                  <Routes>
                    <Route path="/" element={<Navigate to="/dashboard" replace />} />
          <Route path="/dashboard" element={<DashboardScreen />} />
          <Route path="/Department" element={<DepartmentList />} />
          <Route path="/Branch" element={<BranchList />} />
          <Route path="/Employee" element={<EmployeeList />} />
          <Route path="/EmployeeCertificate" element={<EmployeeCertificateList />} />
          <Route path="/DisciplinaryRecord" element={<DisciplinaryRecordList />} />
          <Route path="/OvertimeRecord" element={<OvertimeRecordList />} />
          <Route path="/LeaveType" element={<LeaveTypeList />} />
          <Route path="/LeaveRequest" element={<LeaveRequestList />} />
          <Route path="/PerformanceReview" element={<PerformanceReviewList />} />
          <Route path="/PerformanceGoal" element={<PerformanceGoalList />} />
          <Route path="/PeerReview" element={<PeerReviewList />} />
          <Route path="/JobPosting" element={<JobPostingList />} />
          <Route path="/JobApplication" element={<JobApplicationList />} />
          <Route path="/Onboarding" element={<OnboardingList />} />
          <Route path="/OnboardingTask" element={<OnboardingTaskList />} />
          <Route path="/SalaryRecord" element={<SalaryRecordList />} />
          <Route path="/SalaryDeduction" element={<SalaryDeductionList />} />
          <Route path="/TrainingPlan" element={<TrainingPlanList />} />
          <Route path="/Training" element={<TrainingList />} />
          <Route path="/TrainingParticipation" element={<TrainingParticipationList />} />
          <Route path="/tasks" element={<TaskInboxScreen />} />
          <Route path="/users" element={<UserListScreen />} />
          <Route path="/roles" element={<RoleListScreen />} />
                  </Routes>
                </GlobalMenu>
              )
              : <LoginPage onLogin={() => setAuthenticated(true)} />
          } />
        </Routes>
      </BrowserRouter>
        </FlowProvider>
    </QueryClientProvider>
  );
};
