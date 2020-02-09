using AutoMapper;
using SE.Core.DTO;
using SE.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Infrastructure.AutoMapper
{
    public class AutoMapping:Profile
    {
        public AutoMapping()
        {
            CreateMap<EducationInsertModel, EducationInsertDto>();
            CreateMap<EducationGeneralInformationModel, EducationGeneralInformationDto>();
            CreateMap<EducationQuestionModel, EducationQuestionDto>();
            CreateMap<EducationAddressModel, EducationAddressDto>();
            CreateMap<EducationContactInformationModel, EducationContactInformationDto>();
        }
    }
}
