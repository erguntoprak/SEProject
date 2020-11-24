using SE.Core.DTO;
using SE.Core.Entities;
using SE.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE.Business.EducationServices
{
    public interface IEducationService
    {
        Task<IDataResult<int>> InsertEducationAsync(EducationInsertDto educationInsertDto);
        Task<IDataResult<IEnumerable<EducationListDto>>> GetAllEducationListByUserIdAsync(string userId);
        Task<IDataResult<IEnumerable<EducationListDto>>> GetAllEducationListByCategoryIdAndDistrictIdAsync(int categoryId, int districtId);
        Task<IDataResult<IEnumerable<EducationListDto>>> GetAllEducationListAsync();
        Task<IDataResult<EducationUpdateDto>> GetEducationUpdateDtoBySeoUrlAsync(string seoUrl, string userId);
        Task<IDataResult<int>> UpdateEducationAsync(EducationUpdateDto educationUpdateDto);
        Task<IResult> DeleteEducationAsync(int educationId,string userId);
        Task<IDataResult<EducationDetailDto>> GetEducationDetailDtoBySeoUrlAsync(string seoUrl);
        Task<IDataResult<IEnumerable<ImageDto>>> GetAllEducationImageDtoByEducationIdAsync(int educationId);
        Task<IResult> InsertFirstVisibleImageAsync(ImageDto imageDto);
        Task<IResult> UpdateFirstVisibleImageAsync(ImageDto imageDto);
        Task<IResult> InsertEducationContactFormAsync(EducationContactFormInsertDto educationContactFormDto);
        Task<IDataResult<IEnumerable<EducationContactFormListDto>>> GetEducationContactFormListDtoBySeoUrlAsync(string seoUrl, string userId);
        Task<IDataResult<IEnumerable<SearchEducationDto>>> GetAllSearchEducationListAsync();
        Task<IDataResult<IEnumerable<EducationFilterListDto>>> GetAllEducationListByFilterAsync(FilterDto filterDto);
        Task<IResult> UploadEducationImageAsync(EducationUploadImageDto educationUploadImageDto);
        Task<IResult> UpdateEducationActivateAsync(int educationId, bool isActive);

    }
}
