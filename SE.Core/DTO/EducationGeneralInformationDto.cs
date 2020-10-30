using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class EducationGeneralInformationDto
    {
        public int Id { get; set; }
        public string EducationName { get; set; }
        public int EducationType { get; set; }
        public string Description { get; set; }
        public string SeoUrl { get; set; }
        public string CategoryName { get; set; }
        public string CategorySeoUrl { get; set; }

    }
}
