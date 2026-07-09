using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.OvertimeRecords.Dto
{
    [AutoMapTo(typeof(Entities.OvertimeRecord))]
    public class CreateOvertimeRecordDto
    {
        public DateTime OvertimeDate { get; set; }

        public decimal Hours { get; set; }

        [MaxLength(500)]
        public string Reason { get; set; }

        public int Status { get; set; }

        [MaxLength(500)]
        public string ApproverNote { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public long? ManagerApproverId { get; set; }

        public long EmployeeId { get; set; }

    }
}