using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model.Blog
{
    public class BlogUpdateModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstVisibleImageName { get; set; }
        public List<BlogItemModel> BlogItems { get; set; }
    }
}
