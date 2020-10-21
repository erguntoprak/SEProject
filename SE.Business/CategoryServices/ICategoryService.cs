using SE.Core.DTO;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.CategoryServices
{
    public interface ICategoryService
    {
        List<CategoryDto> GetAllCategoryList();
        CategoryDto GetCategoryById(int categoryId);
        void UpdateCategory(CategoryDto categoryDto);
        void InsertCategory(CategoryDto categoryDto);
        void DeleteCategory(int categoryId);
    }
}
