using SE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.Business.ImageServices
{
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void DeleteImageByImageUrl(string imageUrl)
        {
            try
            {
                var image = _unitOfWork.ImageRepository.Table.FirstOrDefault(d => d.ImageUrl == imageUrl);
                if (image != null)
                {
                    _unitOfWork.ImageRepository.Delete(image);
                    _unitOfWork.SaveChanges();
                }
            }
            catch
            {

                throw;
            }
        }

        public List<string> GetImagesByEducationId(int educationId)
        {
            try
            {
                var images = _unitOfWork.ImageRepository.Table.Where(d => d.EducationId == educationId).Select(d => d.ImageUrl).ToList();
                return images;
            }
            catch
            {
                throw;
            }
        }
    }
}
