using SE.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.AttributeServices
{
    public interface IAttributeService
    {
        List<CategoryAttributeListDto> GetAllAttributeByEducationCategoryId(int categoryId);
        List<AttributeListDto> GetAllAttributeList();
        AttributeDto GetAttributeById(int attributeId);
        void UpdateAttribute(AttributeDto attributeDto);
        void InsertAttribute(AttributeDto attributeDto);
        void DeleteAttribute(int attributeId);
    }
}
