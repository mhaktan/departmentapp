// Auto-generated flow: PerformanceReview Approval Flow
// Auto-generated approval flow for PerformanceReview. Customize email templates and add conditions as needed.
// Resource: PerformanceReview
// Enabled: true
//
// Nodes:
  // trigger: On PerformanceReview Submit
  // condition: Status = SelfAssessmentPending?
  // approval: PerformanceReview Approval
  // action: Send Approval Email
  // trigger: On PerformanceReview Approved
  // action: Send Completion Email
//
// Edges:
  // On PerformanceReview Submit → Status = SelfAssessmentPending?
  // Status = SelfAssessmentPending? → PerformanceReview Approval (true)
  // PerformanceReview Approval → Send Approval Email
  // On PerformanceReview Approved → Send Completion Email
//
// This file is for documentation purposes.
// Flow execution is handled by FlowEngine.ts using flowDefinitions.json.

export const FLOW_PERFORMANCEREVIEW_APPROVAL_FLOW_ID = 'flow-PerformanceReview-approval';
