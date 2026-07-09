using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.JobPostings.Dto;

namespace DepartmentApp.JobPostings
{
    public class JobPostingMapProfile : Profile
    {
        public JobPostingMapProfile()
        {
            CreateMap<JobPosting, JobPostingDto>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<CreateJobPostingDto, JobPosting>();
            CreateMap<JobPostingDto, JobPosting>();
        }
    }
}
