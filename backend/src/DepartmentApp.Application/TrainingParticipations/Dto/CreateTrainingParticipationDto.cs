using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.TrainingParticipations.Dto
{
    [AutoMapTo(typeof(Entities.TrainingParticipation))]
    public class CreateTrainingParticipationDto
    {
        public bool Attended { get; set; }

        public DateTime? CompletionDate { get; set; }

        public decimal? Score { get; set; }

        public bool? CertificateEarned { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public long TrainingId { get; set; }

        public long EmployeeId { get; set; }

    }
}