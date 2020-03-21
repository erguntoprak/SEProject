using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.ImageServices
{
    public interface IImageService
    {
        List<string> GetImagesByEducationId(int educationId);
        void DeleteImageByImageUrl(string imageUrl);
    }
}
