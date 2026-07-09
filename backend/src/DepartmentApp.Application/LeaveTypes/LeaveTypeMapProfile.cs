using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.LeaveTypes.Dto;

namespace DepartmentApp.LeaveTypes
{
    public class LeaveTypeMapProfile : Profile
    {
        public LeaveTypeMapProfile()
        {
            CreateMap<LeaveType, LeaveTypeDto>();
            CreateMap<CreateLeaveTypeDto, LeaveType>();
            CreateMap<LeaveTypeDto, LeaveType>();
        }
    }
}
