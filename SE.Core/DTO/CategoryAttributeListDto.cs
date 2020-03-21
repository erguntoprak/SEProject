using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class CategoryAttributeListDto
    {
        public string CategoryName { get; set; }
        public List<AttributeDto> AttributeListDto { get; set; }
    }
}
