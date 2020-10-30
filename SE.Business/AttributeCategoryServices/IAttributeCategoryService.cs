using SE.Core.DTO;
using SE.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE.Business.AttributeCategoryServices
{
    public interface IAttributeCategoryService
    {
        Task<IDataResult<IEnumerable<AttributeCategoryDto>>> GetAllAttributeCategoryListAsync();
        Task<IDataResult<AttributeCategoryDto>> GetAttributeCategoryByIdAsync(int attributeCategoryId);
        void UpdateAttributeCategory(AttributeCategoryDto attributeCategoryDto);
        void InsertAttributeCategory(AttributeCategoryDto attributeCategoryDto);
        void DeleteAttributeCategory(int attributeCategoryId);
        void InsertCategoryAttributeCategory(CategoryAttributeCategoryInsertDto categoryAttributeCategoryInsertDto);
        int[] GetAttributeCategoryIdsByCategoryId(int categoryId);

    }
}
