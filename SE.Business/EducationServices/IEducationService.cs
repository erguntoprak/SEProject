using SE.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.EducationServices
{
    public interface IEducationService
    {
        void InsertEducation(EducationInsertDto educationInsertDto);
        List<EducationListDto> GetAllEducationListByUserId(string userId);
    }
}
