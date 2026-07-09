using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.PeerReviews.Dto
{
    [AutoMapTo(typeof(Entities.PeerReview))]
    public class CreatePeerReviewDto
    {
        [MaxLength(200)]
        public string ReviewerName { get; set; }

        public decimal? Score { get; set; }

        [MaxLength(2000)]
        public string Strengths { get; set; }

        [MaxLength(2000)]
        public string Improvements { get; set; }

        public bool IsAnonymous { get; set; }

        public long PerformanceReviewId { get; set; }

        public long EmployeeId { get; set; }

    }
}