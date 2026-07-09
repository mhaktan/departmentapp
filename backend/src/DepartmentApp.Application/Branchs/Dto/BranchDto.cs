using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp.Branchs.Dto
{
    [AutoMapFrom(typeof(Entities.Branch))]
    public class BranchDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}