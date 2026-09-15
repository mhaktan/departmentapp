using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.Suppliers.Dto;

namespace DepartmentApp.Suppliers
{
    public class SupplierMapProfile : Profile
    {
        public SupplierMapProfile()
        {
            CreateMap<Supplier, SupplierDto>();
            CreateMap<CreateSupplierDto, Supplier>();
            CreateMap<SupplierDto, Supplier>();
        }
    }
}
