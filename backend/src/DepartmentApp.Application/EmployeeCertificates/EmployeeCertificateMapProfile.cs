using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.EmployeeCertificates.Dto;

namespace DepartmentApp.EmployeeCertificates
{
    public class EmployeeCertificateMapProfile : Profile
    {
        public EmployeeCertificateMapProfile()
        {
            CreateMap<EmployeeCertificate, EmployeeCertificateDto>();
            CreateMap<CreateEmployeeCertificateDto, EmployeeCertificate>();
            CreateMap<EmployeeCertificateDto, EmployeeCertificate>();
        }
    }
}
