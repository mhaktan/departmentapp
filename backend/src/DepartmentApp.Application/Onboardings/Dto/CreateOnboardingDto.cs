using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.Onboardings.Dto
{
    [AutoMapTo(typeof(Entities.Onboarding))]
    public class CreateOnboardingDto
    {
        public DateTime StartDate { get; set; }

        public DateTime? ExpectedCompletionDate { get; set; }

        public int Status { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

        public long JobApplicationId { get; set; }

    }
}