﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class Image:BaseEntity
    {
        public string ImageUrl{ get; set; }
        public string Title { get; set; }
        public bool FirstVisible { get; set; }
        public int EducationId { get; set; }
        public Education Education { get; set; }
    }
}
