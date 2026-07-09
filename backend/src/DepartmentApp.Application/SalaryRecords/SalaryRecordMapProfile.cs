using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.SalaryRecords.Dto;

namespace DepartmentApp.SalaryRecords
{
    public class SalaryRecordMapProfile : Profile
    {
        public SalaryRecordMapProfile()
        {
            CreateMap<SalaryRecord, SalaryRecordDto>();
            CreateMap<CreateSalaryRecordDto, SalaryRecord>();
            CreateMap<SalaryRecordDto, SalaryRecord>();
        }
    }
}
