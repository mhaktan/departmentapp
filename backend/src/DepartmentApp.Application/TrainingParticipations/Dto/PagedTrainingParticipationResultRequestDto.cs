using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.TrainingParticipations.Dto
{
    public class PagedTrainingParticipationResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? TrainingId { get; set; }
        public long? EmployeeId { get; set; }
        public bool? Attended { get; set; }
        public DateTime? CompletionDate { get; set; }
        public decimal? Score { get; set; }
        public bool? CertificateEarned { get; set; }
        public string Notes { get; set; }
    }
}
