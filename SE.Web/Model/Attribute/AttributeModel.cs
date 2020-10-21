using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model.Attribute
{
    public class AttributeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AttributeCategoryId { get; set; }
    }
}
