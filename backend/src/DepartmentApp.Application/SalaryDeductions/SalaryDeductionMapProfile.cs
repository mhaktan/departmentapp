using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.SalaryDeductions.Dto;

namespace DepartmentApp.SalaryDeductions
{
    public class SalaryDeductionMapProfile : Profile
    {
        public SalaryDeductionMapProfile()
        {
            CreateMap<SalaryDeduction, SalaryDeductionDto>();
            CreateMap<CreateSalaryDeductionDto, SalaryDeduction>();
            CreateMap<SalaryDeductionDto, SalaryDeduction>();
        }
    }
}
