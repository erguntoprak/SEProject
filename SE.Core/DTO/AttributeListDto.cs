using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class AttributeListDto
    {
        public string CategoryName { get; set; }
        public List<AttributeDto> AttributeDtoList { get; set; }
        public bool Selected { get; set; }
    }
}
