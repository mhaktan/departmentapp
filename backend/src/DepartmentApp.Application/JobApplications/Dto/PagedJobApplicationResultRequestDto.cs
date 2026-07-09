using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.JobApplications.Dto
{
    public class PagedJobApplicationResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? JobPostingId { get; set; }
        public string ApplicantFirstName { get; set; }
        public string ApplicantLastName { get; set; }
        public string ApplicantEmail { get; set; }
        public string ApplicantPhone { get; set; }
        public string CoverLetter { get; set; }
        public int? Status { get; set; }
        public string ScreeningNotes { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string InterviewNotes { get; set; }
        public decimal? OfferSalary { get; set; }
        public DateTime? OfferDate { get; set; }
        public string RejectionReason { get; set; }
    }
}
