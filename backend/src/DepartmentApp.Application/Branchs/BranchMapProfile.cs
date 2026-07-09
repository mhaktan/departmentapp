using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.Branchs.Dto;

namespace DepartmentApp.Branchs
{
    public class BranchMapProfile : Profile
    {
        public BranchMapProfile()
        {
            CreateMap<Branch, BranchDto>();
            CreateMap<CreateBranchDto, Branch>();
            CreateMap<BranchDto, Branch>();
        }
    }
}
