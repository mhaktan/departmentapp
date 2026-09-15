using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.PurchaseOrders.Dto;

namespace DepartmentApp.PurchaseOrders
{
    public class PurchaseOrderMapProfile : Profile
    {
        public PurchaseOrderMapProfile()
        {
            CreateMap<PurchaseOrder, PurchaseOrderDto>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<CreatePurchaseOrderDto, PurchaseOrder>();
            CreateMap<PurchaseOrderDto, PurchaseOrder>();
        }
    }
}
