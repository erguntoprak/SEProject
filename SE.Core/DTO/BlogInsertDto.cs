using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class BlogInsertDto
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string FirstVisibleImageName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public List<BlogItemDto> BlogItems { get; set; }
    }
}
