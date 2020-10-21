using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model.Attribute
{
    public class CategoryAttributeCategoryInsertModel
    {
        public int CategoryId { get; set; }
        public List<AttributeCategoryModel> AttributeCategoryList { get; set; }
    }
}
