using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.JobPostings.Dto
{
    [AutoMapTo(typeof(Entities.JobPosting))]
    public class CreateJobPostingDto
    {
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [MaxLength(5000)]
        public string Description { get; set; }

        [MaxLength(3000)]
        public string Requirements { get; set; }

        public int PositionCount { get; set; }

        public int Status { get; set; }

        public DateTime? PublishDate { get; set; }

        public DateTime? ClosingDate { get; set; }

        public int EmploymentType { get; set; }

        public long DepartmentId { get; set; }

    }
}