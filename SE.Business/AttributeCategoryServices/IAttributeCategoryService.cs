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
        Task<IResult> UpdateAttributeCategoryAsync(AttributeCategoryDto attributeCategoryDto);
        Task<IResult> InsertAttributeCategoryAsync(AttributeCategoryDto attributeCategoryDto);
        Task<IResult> DeleteAttributeCategoryAsync(int attributeCategoryId);
        Task<IResult> InsertCategoryAttributeCategoryAsync(CategoryAttributeCategoryInsertDto categoryAttributeCategoryInsertDto);
        Task<IDataResult<int[]>>  GetAttributeCategoryIdsByCategoryIdAsync(int categoryId);

    }
}
