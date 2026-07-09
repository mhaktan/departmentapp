using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.TrainingParticipations.Dto;

namespace DepartmentApp.TrainingParticipations
{
    public class TrainingParticipationMapProfile : Profile
    {
        public TrainingParticipationMapProfile()
        {
            CreateMap<TrainingParticipation, TrainingParticipationDto>();
            CreateMap<CreateTrainingParticipationDto, TrainingParticipation>();
            CreateMap<TrainingParticipationDto, TrainingParticipation>();
        }
    }
}
