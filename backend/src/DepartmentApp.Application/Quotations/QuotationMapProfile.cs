using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.Quotations.Dto;

namespace DepartmentApp.Quotations
{
    public class QuotationMapProfile : Profile
    {
        public QuotationMapProfile()
        {
            CreateMap<Quotation, QuotationDto>();
            CreateMap<CreateQuotationDto, Quotation>();
            CreateMap<QuotationDto, Quotation>();
        }
    }
}
