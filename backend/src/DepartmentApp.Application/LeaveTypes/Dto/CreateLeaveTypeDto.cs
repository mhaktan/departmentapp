using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.LeaveTypes.Dto
{
    [AutoMapTo(typeof(Entities.LeaveType))]
    public class CreateLeaveTypeDto
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

    }
}