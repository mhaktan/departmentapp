using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.Trainings.Dto;

namespace DepartmentApp.Trainings
{
    public class TrainingMapProfile : Profile
    {
        public TrainingMapProfile()
        {
            CreateMap<Training, TrainingDto>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<CreateTrainingDto, Training>();
            CreateMap<TrainingDto, Training>();
        }
    }
}
