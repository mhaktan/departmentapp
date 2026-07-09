using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.Departments.Dto
{
    public class PagedDepartmentResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? BranchId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
