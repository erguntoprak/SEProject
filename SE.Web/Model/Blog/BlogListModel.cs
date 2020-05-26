using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model.Blog
{
    public class BlogListModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SeoUrl { get; set; }
        public string Username { get; set; }
        public string FirstVisibleImageName { get; set; }
        public string CreateTime { get; set; }
    }
}
