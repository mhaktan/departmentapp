using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.PeerReviews.Dto
{
    [AutoMapFrom(typeof(Entities.PeerReview))]
    public class PeerReviewDto : EntityDto<long>
    {
        public string ReviewerName { get; set; }

        public decimal? Score { get; set; }

        public string Strengths { get; set; }

        public string Improvements { get; set; }

        public bool IsAnonymous { get; set; }

        public long PerformanceReviewId { get; set; }

        public long EmployeeId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}