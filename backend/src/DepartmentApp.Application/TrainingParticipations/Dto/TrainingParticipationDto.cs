using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.TrainingParticipations.Dto
{
    [AutoMapFrom(typeof(Entities.TrainingParticipation))]
    public class TrainingParticipationDto : EntityDto<long>
    {
        public bool Attended { get; set; }

        public DateTime? CompletionDate { get; set; }

        public decimal? Score { get; set; }

        public bool? CertificateEarned { get; set; }

        public string Notes { get; set; }

        public long TrainingId { get; set; }

        public long EmployeeId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}