using SE.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.AttributeCategoryServices
{
    public interface IAttributeCategoryService
    {
        List<AttributeCategoryDto> GetAllAttributeCategoryList();
        AttributeCategoryDto GetAttributeCategoryById(int attributeCategoryId);
        void UpdateAttributeCategory(AttributeCategoryDto attributeCategoryDto);
        void InsertAttributeCategory(AttributeCategoryDto attributeCategoryDto);
        void DeleteAttributeCategory(int attributeCategoryId);
        void InsertCategoryAttributeCategory(CategoryAttributeCategoryInsertDto categoryAttributeCategoryInsertDto);
        int[] GetAttributeCategoryIdsByCategoryId(int categoryId);

    }
}
