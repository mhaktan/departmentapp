using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.OvertimeRecords.Dto
{
    public class PagedOvertimeRecordResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? EmployeeId { get; set; }
        public DateTime? OvertimeDate { get; set; }
        public decimal? Hours { get; set; }
        public string Reason { get; set; }
        public int? Status { get; set; }
        public string ApproverNote { get; set; }
        public string Notes { get; set; }
        public long? ManagerApproverId { get; set; }
    }
}
