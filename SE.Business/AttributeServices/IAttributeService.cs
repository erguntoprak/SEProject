using SE.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.AttributeServices
{
    public interface IAttributeService
    {
        List<AttributeListDto> GetAllEducationAttributeList();
    }
}
