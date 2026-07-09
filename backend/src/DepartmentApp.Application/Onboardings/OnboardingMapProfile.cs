using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.Onboardings.Dto;

namespace DepartmentApp.Onboardings
{
    public class OnboardingMapProfile : Profile
    {
        public OnboardingMapProfile()
        {
            CreateMap<Onboarding, OnboardingDto>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<CreateOnboardingDto, Onboarding>();
            CreateMap<OnboardingDto, Onboarding>();
        }
    }
}
