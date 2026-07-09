using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.DisciplinaryRecords.Dto;

namespace DepartmentApp.DisciplinaryRecords
{
    public class DisciplinaryRecordMapProfile : Profile
    {
        public DisciplinaryRecordMapProfile()
        {
            CreateMap<DisciplinaryRecord, DisciplinaryRecordDto>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<CreateDisciplinaryRecordDto, DisciplinaryRecord>();
            CreateMap<DisciplinaryRecordDto, DisciplinaryRecord>();
        }
    }
}
