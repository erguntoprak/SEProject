using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class Blog : BaseEntity
    {
        public Blog()
        {
            BlogItems = new List<BlogItem>();
        }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string FirstVisibleImageName { get; set; }
        public string SeoUrl { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public bool IsActive { get; set; }
        public User User { get; set; }
        public ICollection<BlogItem> BlogItems { get; set; }

    }
}
