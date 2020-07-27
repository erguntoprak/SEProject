using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class EducationInsertDto
    {
        public EducationGeneralInformationDto GeneralInformation { get; set; }
        public int[] Attributes { get; set; }
        public string[] Images { get; set; }
        public string UserId { get; set; }
        public List<EducationQuestionDto> Questions { get; set; }
        public EducationAddressDto AddressInformation { get; set; }
        public SocialInformationDto SocialInformation { get; set; }
        public EducationContactInformationDto ContactInformation { get; set; }
    }
}
