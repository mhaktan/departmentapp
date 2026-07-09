using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.PerformanceReviews.Dto
{
    [AutoMapFrom(typeof(Entities.PerformanceReview))]
    public class PerformanceReviewDto : EntityDto<long>
    {
        public string ReviewPeriod { get; set; }

        public int ReviewYear { get; set; }

        public int ReviewType { get; set; }

        public int Status { get; set; }

        public decimal? SelfAssessmentScore { get; set; }

        public string SelfAssessmentNotes { get; set; }

        public decimal? ManagerScore { get; set; }

        public string ManagerNotes { get; set; }

        public decimal? OverallScore { get; set; }

        public string HrNotes { get; set; }

        public string RevisionNote { get; set; }

        public long? ManagerReviewerId { get; set; }

        public long? HrReviewerId { get; set; }

        public long? PeerReviewersAssignedBy { get; set; }

        /// <summary>
        /// String form of the status — used by flow conditions (triggerData.statusName equals "PendingX").
        /// </summary>
        public string StatusName { get; set; }

        public long EmployeeId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}