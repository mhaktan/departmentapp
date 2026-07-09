using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.PeerReviews.Dto;

namespace DepartmentApp.PeerReviews
{
    public class PeerReviewMapProfile : Profile
    {
        public PeerReviewMapProfile()
        {
            CreateMap<PeerReview, PeerReviewDto>();
            CreateMap<CreatePeerReviewDto, PeerReview>();
            CreateMap<PeerReviewDto, PeerReview>();
        }
    }
}
