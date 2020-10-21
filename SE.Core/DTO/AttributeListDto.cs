using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class AttributeListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AttributeCategoryId { get; set; }
        public string AttributeCategoryName { get; set; }

    }
}
