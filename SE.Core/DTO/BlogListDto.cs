using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class BlogListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UserSeoUrl { get; set; }
        public string SeoUrl { get; set; }
        public string UserName { get; set; }
        public string FirstVisibleImageName { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
