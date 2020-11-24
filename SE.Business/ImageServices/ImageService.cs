using Microsoft.EntityFrameworkCore;
using SE.Business.Constants;
using SE.Core.Utilities.Results;
using SE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.Business.ImageServices
{
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult> DeleteImageByImageUrlAsync(string imageUrl)
        {
            var image = await _unitOfWork.ImageRepository.Table.FirstOrDefaultAsync(d => d.ImageUrl == imageUrl);
            if (image == null)
                return new ErrorResult(Messages.ObjectIsNull);

            _unitOfWork.ImageRepository.Delete(image);
            await _unitOfWork.SaveChangesAsync();

            return new SuccessResult(Messages.Deleted);
        }

        public async Task<IDataResult<IEnumerable<string>>> GetImagesByEducationIdAsync(int educationId)
        {
            var images = await _unitOfWork.ImageRepository.Table.Where(d => d.EducationId == educationId).Select(d => d.ImageUrl).ToListAsync();
            return new SuccessDataResult<IEnumerable<string>>(images);
        }
    }
}
