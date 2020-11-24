using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class EducationUploadImageDto
    {
        public IFormFileCollection UploadImages { get; set; }
        public int EducationId { get; set; }
        public string Path { get; set; }
    }
}
