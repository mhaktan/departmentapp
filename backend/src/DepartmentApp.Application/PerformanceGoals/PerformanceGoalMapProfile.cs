using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.PerformanceGoals.Dto;

namespace DepartmentApp.PerformanceGoals
{
    public class PerformanceGoalMapProfile : Profile
    {
        public PerformanceGoalMapProfile()
        {
            CreateMap<PerformanceGoal, PerformanceGoalDto>();
            CreateMap<CreatePerformanceGoalDto, PerformanceGoal>();
            CreateMap<PerformanceGoalDto, PerformanceGoal>();
        }
    }
}
