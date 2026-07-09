using AutoMapper;
using DepartmentApp.Approvals.Dto;
using DepartmentApp.Entities;

namespace DepartmentApp.Approvals
{
    public class ApprovalMapProfile : Profile
    {
        public ApprovalMapProfile()
        {
            CreateMap<ApprovalRecord, ApprovalRecordDto>();
            CreateMap<StatusChangeLog, StatusChangeLogDto>();
        }
    }
}
