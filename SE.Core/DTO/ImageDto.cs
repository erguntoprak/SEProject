using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class ImageDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public bool FirstVisible { get; set; }
        public int EducationId { get; set; }
    }
}
