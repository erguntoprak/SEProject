using SE.Core.DTO;
using SE.Core.Entities;
using SE.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE.Business.CategoryServices
{
    public interface ICategoryService
    {
        Task<IDataResult<IEnumerable<CategoryDto>>> GetAllCategoryListAsync();
        Task<IDataResult<CategoryDto>> GetCategoryByIdAsync(int categoryId);
        Task<IResult> UpdateCategoryAsync(CategoryDto categoryDto);
        Task<IResult> InsertCategoryAsync(CategoryDto categoryDto);
        Task<IResult> DeleteCategoryAsync(int categoryId);
    }
}
