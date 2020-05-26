﻿using System;
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
        }
        public EducationGeneralInformationDto GeneralInformation { get; set; }
        public int[] Attributes { get; set; }
        public string[] Images { get; set; }
        public string UserId { get; set; }

        public List<EducationQuestionDto> Questions { get; set; }
        public EducationAddressDto AddressInformation { get; set; }
        public EducationContactInformationDto ContactInformation { get; set; }
    }
}