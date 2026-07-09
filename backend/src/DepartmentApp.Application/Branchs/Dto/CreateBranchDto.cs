using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp.Branchs.Dto
{
    [AutoMapTo(typeof(Entities.Branch))]
    public class CreateBranchDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

    }
}