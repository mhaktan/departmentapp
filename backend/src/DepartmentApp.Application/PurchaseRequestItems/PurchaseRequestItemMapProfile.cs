using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.PurchaseRequestItems.Dto;

namespace DepartmentApp.PurchaseRequestItems
{
    public class PurchaseRequestItemMapProfile : Profile
    {
        public PurchaseRequestItemMapProfile()
        {
            CreateMap<PurchaseRequestItem, PurchaseRequestItemDto>();
            CreateMap<CreatePurchaseRequestItemDto, PurchaseRequestItem>();
            CreateMap<PurchaseRequestItemDto, PurchaseRequestItem>();
        }
    }
}
