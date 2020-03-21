using SE.Web.Model.Attribute;
using System.Collections.Generic;

namespace SE.Web.Model.Category
{
    public class CategoryAttributeListModel
    {
        public string CategoryName { get; set; }
        public List<AttributeModel> AttributeListModel { get; set; }
    }
}
