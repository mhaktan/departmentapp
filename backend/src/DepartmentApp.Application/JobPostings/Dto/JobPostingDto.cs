using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.JobPostings.Dto
{
    [AutoMapFrom(typeof(Entities.JobPosting))]
    public class JobPostingDto : EntityDto<long>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Requirements { get; set; }

        public int PositionCount { get; set; }

        public int Status { get; set; }

        public DateTime? PublishDate { get; set; }

        public DateTime? ClosingDate { get; set; }

        public int EmploymentType { get; set; }

        /// <summary>
        /// String form of the status — used by flow conditions (triggerData.statusName equals "PendingX").
        /// </summary>
        public string StatusName { get; set; }

        public long DepartmentId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}