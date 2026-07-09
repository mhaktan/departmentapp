using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.LeaveTypes.Dto
{
    [AutoMapFrom(typeof(Entities.LeaveType))]
    public class LeaveTypeDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public bool RequiresHRApproval { get; set; }

        public bool IsPaid { get; set; }

        public int? MaxDaysPerYear { get; set; }

        public string Description { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}