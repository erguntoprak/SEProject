using Microsoft.AspNetCore.Http;

namespace SE.Web.Model.Education
{
    public class EducationUploadImageModel
    {
        public IFormFileCollection UploadImages { get; set; }
        public int EducationId { get; set; }
        public string Path { get; set; }
    }
}
