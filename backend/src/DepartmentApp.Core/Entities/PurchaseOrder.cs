using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    // State Machine: status — Draft → PendingApproval → Approved → Rejected
    // Initial: Draft | Transitions: Draft→PendingApproval[Submit], PendingApproval→Approved[Approve], PendingApproval→Draft[Revise], Draft→Rejected[Reject]
    [Table("PurchaseOrders")]
    public class PurchaseOrder : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal OrderAmount { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        [MaxLength(1000)]
        public string RejectionReason { get; set; }

        public PurchaseOrderStatus Status { get; set; }

        public long PurchaseRequestId { get; set; }

        [ForeignKey(nameof(PurchaseRequestId))]
        public virtual PurchaseRequest PurchaseRequest { get; set; }

        public long SupplierId { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public virtual Supplier Supplier { get; set; }

    }
}