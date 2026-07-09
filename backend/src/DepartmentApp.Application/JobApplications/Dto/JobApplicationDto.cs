using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.JobApplications.Dto
{
    [AutoMapFrom(typeof(Entities.JobApplication))]
    public class JobApplicationDto : EntityDto<long>
    {
        public string ApplicantFirstName { get; set; }

        public string ApplicantLastName { get; set; }

        public string ApplicantEmail { get; set; }

        public string ApplicantPhone { get; set; }

        public string CoverLetter { get; set; }

        public int Status { get; set; }

        public string ScreeningNotes { get; set; }

        public DateTime? InterviewDate { get; set; }

        public string InterviewNotes { get; set; }

        public decimal? OfferSalary { get; set; }

        public DateTime? OfferDate { get; set; }

        public string RejectionReason { get; set; }

        /// <summary>
        /// String form of the status — used by flow conditions (triggerData.statusName equals "PendingX").
        /// </summary>
        public string StatusName { get; set; }

        public long JobPostingId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}