using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.Employees.Dto;

namespace DepartmentApp.Employees
{
    public class EmployeeMapProfile : Profile
    {
        public EmployeeMapProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<EmployeeDto, Employee>();
        }
    }
}
