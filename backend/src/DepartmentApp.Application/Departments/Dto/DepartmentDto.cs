using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.Departments.Dto
{
    [AutoMapFrom(typeof(Entities.Department))]
    public class DepartmentDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public long BranchId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}