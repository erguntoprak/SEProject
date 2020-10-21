using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class CategoryAttributeCategoryInsertDto
    {
        public int CategoryId { get; set; }
        public List<AttributeCategoryDto> AttributeCategoryList { get; set; }
    }
}
