using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.JobApplications.Dto;

namespace DepartmentApp.JobApplications
{
    public class JobApplicationMapProfile : Profile
    {
        public JobApplicationMapProfile()
        {
            CreateMap<JobApplication, JobApplicationDto>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<CreateJobApplicationDto, JobApplication>();
            CreateMap<JobApplicationDto, JobApplication>();
        }
    }
}
