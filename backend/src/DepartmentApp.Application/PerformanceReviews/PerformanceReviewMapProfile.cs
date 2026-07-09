using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.PerformanceReviews.Dto;

namespace DepartmentApp.PerformanceReviews
{
    public class PerformanceReviewMapProfile : Profile
    {
        public PerformanceReviewMapProfile()
        {
            CreateMap<PerformanceReview, PerformanceReviewDto>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<CreatePerformanceReviewDto, PerformanceReview>();
            CreateMap<PerformanceReviewDto, PerformanceReview>();
        }
    }
}
