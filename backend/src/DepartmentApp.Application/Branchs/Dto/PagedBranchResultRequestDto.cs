using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.Branchs.Dto
{
    public class PagedBranchResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
