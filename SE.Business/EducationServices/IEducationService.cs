using SE.Core.DTO;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.EducationServices
{
    public interface IEducationService
    {
        int InsertEducation(EducationInsertDto educationInsertDto);
        List<EducationListDto> GetAllEducationListByUserId(string userId);
        List<EducationListDto> GetAllEducationListByCategoryId(int categoryId);
        EducationUpdateDto GetEducationUpdateDtoBySeoUrl(string seoUrl, string userId);
        int UpdateEducation(EducationUpdateDto educationUpdateDto);
        void DeleteEducation(int educationId,string userId);
        EducationDetailDto GetEducationDetailDtoBySeoUrl(string seoUrl);
        List<ImageDto> GetAllEducationImageDtoByEducationId(int educationId);
        void InsertFirstVisibleImage(ImageDto imageDto);
        void UpdateFirstVisibleImage(ImageDto imageDto);
        void InsertEducationContactForm(EducationContactFormInsertDto educationContactFormDto);
        List<EducationContactFormListDto> GetEducationContactFormListDtoBySeoUrl(string seoUrl, string userId);

    }
}
