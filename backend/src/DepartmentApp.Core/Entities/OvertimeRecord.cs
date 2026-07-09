using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    // State Machine: status — Pending → Approved → Rejected → Cancelled
    // Initial: Pending | Transitions: Pending→Approved[Approve], Pending→Rejected[Reject], *→Cancelled[Cancel]
    [Table("OvertimeRecords")]
    public class OvertimeRecord : FullAuditedEntity<long>
    {
        public DateTime OvertimeDate { get; set; }

        public decimal Hours { get; set; }

        [MaxLength(500)]
        public string Reason { get; set; }

        public OvertimeRecordStatus Status { get; set; }

        [MaxLength(500)]
        public string ApproverNote { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public long? ManagerApproverId { get; set; }

        public long EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

    }
}