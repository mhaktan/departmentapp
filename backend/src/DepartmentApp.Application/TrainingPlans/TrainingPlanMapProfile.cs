using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.TrainingPlans.Dto;

namespace DepartmentApp.TrainingPlans
{
    public class TrainingPlanMapProfile : Profile
    {
        public TrainingPlanMapProfile()
        {
            CreateMap<TrainingPlan, TrainingPlanDto>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<CreateTrainingPlanDto, TrainingPlan>();
            CreateMap<TrainingPlanDto, TrainingPlan>();
        }
    }
}
