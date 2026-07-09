using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.LeaveRequests.Dto
{
    [AutoMapFrom(typeof(Entities.LeaveRequest))]
    public class LeaveRequestDto : EntityDto<long>
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal TotalDays { get; set; }

        public string Reason { get; set; }

        public int Status { get; set; }

        public string RevisionNote { get; set; }

        public bool RequiresHRApproval { get; set; }

        public long? ManagerApproverId { get; set; }

        public long? HrApproverId { get; set; }

        public bool? BalanceDeducted { get; set; }

        /// <summary>
        /// String form of the status — used by flow conditions (triggerData.statusName equals "PendingX").
        /// </summary>
        public string StatusName { get; set; }

        public long EmployeeId { get; set; }

        public long LeaveTypeId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}