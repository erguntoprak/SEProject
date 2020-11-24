using SE.Core.DTO;
using SE.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE.Business.AttributeServices
{
    public interface IAttributeService
    {
        Task<IDataResult<IEnumerable<CategoryAttributeListDto>>> GetAllAttributeByEducationCategoryIdAsync(int categoryId);
        Task<IDataResult<IEnumerable<AttributeListDto>>> GetAllAttributeListAsync();
        Task<IDataResult<AttributeDto>> GetAttributeByIdAsync(int attributeId);
        Task<IResult> UpdateAttributeAsync(AttributeDto attributeDto);
        Task<IResult> InsertAttributeAsync(AttributeDto attributeDto);
        Task<IResult> DeleteAttributeAsync(int attributeId);
    }
}
