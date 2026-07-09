using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.LeaveRequests.Dto
{
    [AutoMapTo(typeof(Entities.LeaveRequest))]
    public class CreateLeaveRequestDto
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal TotalDays { get; set; }

        [MaxLength(1000)]
        public string Reason { get; set; }

        public int Status { get; set; }

        [MaxLength(1000)]
        public string RevisionNote { get; set; }

        public bool RequiresHRApproval { get; set; }

        public long? ManagerApproverId { get; set; }

        public long? HrApproverId { get; set; }

        public bool? BalanceDeducted { get; set; }

        public long EmployeeId { get; set; }

        public long LeaveTypeId { get; set; }

    }
}