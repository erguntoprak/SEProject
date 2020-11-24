using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model.Blog
{
    public class BlogActivateModel
    {
        public int BlogId { get; set; }
        public bool IsActive { get; set; }
    }
}
