using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model
{
    public class EducationInsertModel
    {
        public EducationGeneralInformationModel GeneralInformation { get; set; }
        public int[] Attributes { get; set; }
        public string[] Images { get; set; }
        public List<EducationQuestionModel> Questions { get; set; }
        public EducationAddressModel AddressInformation { get; set; }
        public EducationContactInformationModel ContactInformation { get; set; }
    }
}
