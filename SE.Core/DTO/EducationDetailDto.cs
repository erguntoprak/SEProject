using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class EducationDetailDto
    {
        public EducationDetailDto()
        {
            GeneralInformation = new EducationGeneralInformationDto();
            Questions = new List<EducationQuestionDto>();
            AddressInformation = new EducationAddressDetailDto();
            ContactInformation = new EducationContactInformationDto();
            SocialInformation = new SocialInformationDto();
        }
        public EducationGeneralInformationDto GeneralInformation { get; set; }
        public List<CategoryAttributeListDto> CategoryAttributeList { get; set; }
        public string[] Images { get; set; }

        public List<EducationQuestionDto> Questions { get; set; }
        public EducationAddressDetailDto AddressInformation { get; set; }
        public SocialInformationDto SocialInformation { get; set; }
        public EducationContactInformationDto ContactInformation { get; set; }
        public List<BlogListDto> BlogList { get; set; }
    }
}
