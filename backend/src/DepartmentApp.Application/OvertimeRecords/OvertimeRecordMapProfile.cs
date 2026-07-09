using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.OvertimeRecords.Dto;

namespace DepartmentApp.OvertimeRecords
{
    public class OvertimeRecordMapProfile : Profile
    {
        public OvertimeRecordMapProfile()
        {
            CreateMap<OvertimeRecord, OvertimeRecordDto>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<CreateOvertimeRecordDto, OvertimeRecord>();
            CreateMap<OvertimeRecordDto, OvertimeRecord>();
        }
    }
}
