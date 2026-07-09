using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DepartmentApp.Entities
{
    [Table("LeaveTypes")]
    public class LeaveType : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        public bool RequiresHRApproval { get; set; }

        public bool IsPaid { get; set; }

        public int? MaxDaysPerYear { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }

    }
}