using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class BlogItem:BaseEntity
    {
        public int BlogId { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
        public Blog Blog { get; set; }

    }
}
