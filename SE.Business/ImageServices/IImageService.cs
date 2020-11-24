using SE.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE.Business.ImageServices
{
    public interface IImageService
    {
        Task<IDataResult<IEnumerable<string>>> GetImagesByEducationIdAsync(int educationId);
        Task<IResult> DeleteImageByImageUrlAsync(string imageUrl);
    }
}
