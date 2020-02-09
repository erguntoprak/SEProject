using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class Question:BaseEntity
    {
        public string Title{ get; set; }
        public string Answer { get; set; }
        public int EducationId { get; set; }
        public Education Education { get; set; }
    }
}
