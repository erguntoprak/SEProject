using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class EducationUpdateDto
    {
        public EducationUpdateDto()
        {
            GeneralInformation = new EducationGeneralInformationDto();
            Questions = new List<EducationQuestionDto>();
            AddressInformation = new EducationAddressDto();
            ContactInformation = new EducationContactInformationDto();
            SocialInformation = new SocialInformationDto();
        }
        public EducationGeneralInformationDto GeneralInformation { get; set; }
        public int[] Attributes { get; set; }
        public string[] Images { get; set; }

        public List<EducationQuestionDto> Questions { get; set; }
        public EducationAddressDto AddressInformation { get; set; }
        public SocialInformationDto SocialInformation { get; set; }
        public EducationContactInformationDto ContactInformation { get; set; }
    }
}
