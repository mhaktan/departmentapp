using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp.ExpenseCategorys.Dto
{
    public class PagedExpenseCategoryResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
