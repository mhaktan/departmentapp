using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.Departments.Dto;

namespace DepartmentApp.Departments
{
    public class DepartmentMapProfile : Profile
    {
        public DepartmentMapProfile()
        {
            CreateMap<Department, DepartmentDto>();
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<DepartmentDto, Department>();
        }
    }
}
