using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.JobApplications.Dto
{
    [AutoMapTo(typeof(Entities.JobApplication))]
    public class CreateJobApplicationDto
    {
        [Required]
        [MaxLength(100)]
        public string ApplicantFirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string ApplicantLastName { get; set; }

        [Required]
        [MaxLength(256)]
        public string ApplicantEmail { get; set; }

        [MaxLength(50)]
        public string ApplicantPhone { get; set; }

        [MaxLength(3000)]
        public string CoverLetter { get; set; }

        public int Status { get; set; }

        [MaxLength(1000)]
        public string ScreeningNotes { get; set; }

        public DateTime? InterviewDate { get; set; }

        [MaxLength(2000)]
        public string InterviewNotes { get; set; }

        public decimal? OfferSalary { get; set; }

        public DateTime? OfferDate { get; set; }

        [MaxLength(500)]
        public string RejectionReason { get; set; }

        public long JobPostingId { get; set; }

    }
}