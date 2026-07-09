// Auto-generated flow: OvertimeRecord Approval Flow
// Auto-generated approval flow for OvertimeRecord. Customize email templates and add conditions as needed.
// Resource: OvertimeRecord
// Enabled: true
//
// Nodes:
  // trigger: On OvertimeRecord Submit
  // condition: Status = Approved?
  // approval: OvertimeRecord Approval
  // action: Send Approval Email
  // trigger: On OvertimeRecord Approved
  // action: Send Completion Email
//
// Edges:
  // On OvertimeRecord Submit → Status = Approved?
  // Status = Approved? → OvertimeRecord Approval (true)
  // OvertimeRecord Approval → Send Approval Email
  // On OvertimeRecord Approved → Send Completion Email
//
// This file is for documentation purposes.
// Flow execution is handled by FlowEngine.ts using flowDefinitions.json.

export const FLOW_OVERTIMERECORD_APPROVAL_FLOW_ID = 'flow-OvertimeRecord-approval';
