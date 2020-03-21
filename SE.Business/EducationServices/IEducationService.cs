using SE.Core.DTO;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.EducationServices
{
    public interface IEducationService
    {
        void InsertEducation(EducationInsertDto educationInsertDto);
        List<EducationListDto> GetAllEducationListByUserId(string userId);
        EducationUpdateDto GetEducationUpdateDtoEditDtoBySeoUrl(string seoUrl, string userId);
        void UpdateEducation(EducationUpdateDto educationUpdateDto);
        void DeleteEducation(int educationId,string userId);
    }
}
