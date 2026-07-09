using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.DisciplinaryRecords.Dto
{
    public class PagedDisciplinaryRecordResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? EmployeeId { get; set; }
        public DateTime? IncidentDate { get; set; }
        public int? Type { get; set; }
        public string Description { get; set; }
        public string ActionTaken { get; set; }
        public string IssuedBy { get; set; }
        public bool? AcknowledgedByEmployee { get; set; }
        public int? Status { get; set; }
        public string AppealNote { get; set; }
        public string ResolutionNote { get; set; }
        public long? HrReviewerId { get; set; }
        public long? HrManagerResolverId { get; set; }
    }
}
