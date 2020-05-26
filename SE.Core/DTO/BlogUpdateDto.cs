using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class BlogUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public string FirstVisibleImageName { get; set; }
        public List<BlogItemDto> BlogItems { get; set; }
    }
}
