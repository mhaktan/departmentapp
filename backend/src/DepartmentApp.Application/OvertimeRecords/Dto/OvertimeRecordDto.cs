using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.OvertimeRecords.Dto
{
    [AutoMapFrom(typeof(Entities.OvertimeRecord))]
    public class OvertimeRecordDto : EntityDto<long>
    {
        public DateTime OvertimeDate { get; set; }

        public decimal Hours { get; set; }

        public string Reason { get; set; }

        public int Status { get; set; }

        public string ApproverNote { get; set; }

        public string Notes { get; set; }

        public long? ManagerApproverId { get; set; }

        /// <summary>
        /// String form of the status — used by flow conditions (triggerData.statusName equals "PendingX").
        /// </summary>
        public string StatusName { get; set; }

        public long EmployeeId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}