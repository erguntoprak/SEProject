using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model.Education
{
    public class EducationUpdateModel
    {
        public EducationGeneralInformationModel GeneralInformation { get; set; }
        public int[] Attributes { get; set; }
        public string[] Images { get; set; }
        public string UserId { get; set; }

        public List<EducationQuestionModel> Questions { get; set; }
        public EducationAddressModel AddressInformation { get; set; }
        public SocialInformationModel SocialInformation { get; set; }
        public EducationContactInformationModel ContactInformation { get; set; }
    }
}
