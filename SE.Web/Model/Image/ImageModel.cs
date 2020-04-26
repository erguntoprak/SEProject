using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model.Image
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public bool FirstVisible { get; set; }
        public int EducationId { get; set; }
    }
}
