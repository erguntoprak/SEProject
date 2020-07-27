using SE.Web.Model.Blog;
using SE.Web.Model.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model.Education
{
    public class EducationDetailModel
    {
        public EducationDetailModel()
        {
            GeneralInformation = new EducationGeneralInformationModel();
            Questions = new List<EducationQuestionModel>();
            AddressInformation = new EducationAddressDetailModel();
            ContactInformation = new EducationContactInformationModel();
        }
        public EducationGeneralInformationModel GeneralInformation { get; set; }
        public List<CategoryAttributeListModel> CategoryAttributeList { get; set; }
        public string[] Images { get; set; }
        public string UserId { get; set; }

        public List<EducationQuestionModel> Questions { get; set; }
        public EducationAddressDetailModel AddressInformation { get; set; }
        public SocialInformationModel SocialInformation { get; set; }
        public EducationContactInformationModel ContactInformation { get; set; }
        public List<BlogListModel> BlogList { get; set; }

    }
}
