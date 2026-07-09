// Auto-generated flow: DisciplinaryRecord Approval Flow
// Auto-generated approval flow for DisciplinaryRecord. Customize email templates and add conditions as needed.
// Resource: DisciplinaryRecord
// Enabled: true
//
// Nodes:
  // trigger: On DisciplinaryRecord Submit
  // condition: Status = UnderReview?
  // approval: DisciplinaryRecord Approval
  // action: Send Approval Email
  // trigger: On DisciplinaryRecord Approved
  // action: Send Completion Email
//
// Edges:
  // On DisciplinaryRecord Submit → Status = UnderReview?
  // Status = UnderReview? → DisciplinaryRecord Approval (true)
  // DisciplinaryRecord Approval → Send Approval Email
  // On DisciplinaryRecord Approved → Send Completion Email
//
// This file is for documentation purposes.
// Flow execution is handled by FlowEngine.ts using flowDefinitions.json.

export const FLOW_DISCIPLINARYRECORD_APPROVAL_FLOW_ID = 'flow-DisciplinaryRecord-approval';
