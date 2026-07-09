using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.DisciplinaryRecords.Dto
{
    [AutoMapFrom(typeof(Entities.DisciplinaryRecord))]
    public class DisciplinaryRecordDto : EntityDto<long>
    {
        public DateTime IncidentDate { get; set; }

        public int Type { get; set; }

        public string Description { get; set; }

        public string ActionTaken { get; set; }

        public string IssuedBy { get; set; }

        public bool AcknowledgedByEmployee { get; set; }

        public int Status { get; set; }

        public string AppealNote { get; set; }

        public string ResolutionNote { get; set; }

        public long? HrReviewerId { get; set; }

        public long? HrManagerResolverId { get; set; }

        /// <summary>
        /// String form of the status — used by flow conditions (triggerData.statusName equals "PendingX").
        /// </summary>
        public string StatusName { get; set; }

        public long EmployeeId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}