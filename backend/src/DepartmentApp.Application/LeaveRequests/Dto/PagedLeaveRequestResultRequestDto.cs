using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.LeaveRequests.Dto
{
    public class PagedLeaveRequestResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? EmployeeId { get; set; }
        public long? LeaveTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? TotalDays { get; set; }
        public string Reason { get; set; }
        public int? Status { get; set; }
        public string RevisionNote { get; set; }
        public bool? RequiresHRApproval { get; set; }
        public long? ManagerApproverId { get; set; }
        public long? HrApproverId { get; set; }
        public bool? BalanceDeducted { get; set; }
    }
}
