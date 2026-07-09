using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.LeaveTypes.Dto
{
    public class PagedLeaveTypeResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool? RequiresHRApproval { get; set; }
        public bool? IsPaid { get; set; }
        public int? MaxDaysPerYear { get; set; }
        public string Description { get; set; }
    }
}
