using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.DisciplinaryRecords.Dto
{
    [AutoMapTo(typeof(Entities.DisciplinaryRecord))]
    public class CreateDisciplinaryRecordDto
    {
        public DateTime IncidentDate { get; set; }

        public int Type { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        [MaxLength(1000)]
        public string ActionTaken { get; set; }

        [MaxLength(200)]
        public string IssuedBy { get; set; }

        public bool AcknowledgedByEmployee { get; set; }

        public int Status { get; set; }

        [MaxLength(1000)]
        public string AppealNote { get; set; }

        [MaxLength(1000)]
        public string ResolutionNote { get; set; }

        public long? HrReviewerId { get; set; }

        public long? HrManagerResolverId { get; set; }

        public long EmployeeId { get; set; }

    }
}