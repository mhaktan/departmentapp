using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.OnboardingTasks.Dto;

namespace DepartmentApp.OnboardingTasks
{
    public class OnboardingTaskMapProfile : Profile
    {
        public OnboardingTaskMapProfile()
        {
            CreateMap<OnboardingTask, OnboardingTaskDto>();
            CreateMap<CreateOnboardingTaskDto, OnboardingTask>();
            CreateMap<OnboardingTaskDto, OnboardingTask>();
        }
    }
}
