using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class Image:BaseEntity
    {
        public string ImageBase64 { get; set; }
        public string Title { get; set; }
        public int EducationId { get; set; }
        public Education Education { get; set; }
    }
}
