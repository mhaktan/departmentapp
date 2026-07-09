using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.LeaveRequests.Dto;

namespace DepartmentApp.LeaveRequests
{
    public class LeaveRequestMapProfile : Profile
    {
        public LeaveRequestMapProfile()
        {
            CreateMap<LeaveRequest, LeaveRequestDto>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<CreateLeaveRequestDto, LeaveRequest>();
            CreateMap<LeaveRequestDto, LeaveRequest>();
        }
    }
}
