using AutoMapper;
using DepartmentApp.Entities;
using DepartmentApp.ExpenseCategorys.Dto;

namespace DepartmentApp.ExpenseCategorys
{
    public class ExpenseCategoryMapProfile : Profile
    {
        public ExpenseCategoryMapProfile()
        {
            CreateMap<ExpenseCategory, ExpenseCategoryDto>();
            CreateMap<CreateExpenseCategoryDto, ExpenseCategory>();
            CreateMap<ExpenseCategoryDto, ExpenseCategory>();
        }
    }
}
