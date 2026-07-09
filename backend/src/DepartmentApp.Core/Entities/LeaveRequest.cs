using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    // State Machine: status — Draft → PendingManagerApproval → PendingHRApproval → Approved → Revision → Cancelled → Rejected
    // Initial: Draft | Transitions: Draft→PendingManagerApproval[Submit], PendingManagerApproval→PendingHRApproval[Approve], PendingManagerApproval→Revision[Revise], PendingManagerApproval→Rejected[Reject], PendingHRApproval→Approved[Approve], PendingHRApproval→Revision[Revise], PendingHRApproval→Rejected[Reject], Revision→PendingManagerApproval[Resubmit], *→Cancelled[Cancel]
    [Table("LeaveRequests")]
    public class LeaveRequest : FullAuditedEntity<long>
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal TotalDays { get; set; }

        [MaxLength(1000)]
        public string Reason { get; set; }

        public LeaveRequestStatus Status { get; set; }

        [MaxLength(1000)]
        public string RevisionNote { get; set; }

        public bool RequiresHRApproval { get; set; }

        public long? ManagerApproverId { get; set; }

        public long? HrApproverId { get; set; }

        public bool? BalanceDeducted { get; set; }

        public long EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        public long LeaveTypeId { get; set; }

        [ForeignKey(nameof(LeaveTypeId))]
        public virtual LeaveType LeaveType { get; set; }

    }
}