using AutoMapper;
using SE.Core.DTO;
using SE.Web.Model;
using SE.Web.Model.Account;
using SE.Web.Model.Address;
using SE.Web.Model.Attribute;
using SE.Web.Model.Category;
using SE.Web.Model.Education;
using SE.Web.Model.Image;
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
            CreateMap<EducationGeneralInformationModel, EducationGeneralInformationDto>().ReverseMap(); ;
            CreateMap<EducationQuestionModel, EducationQuestionDto>().ReverseMap();
            CreateMap<EducationAddressModel, EducationAddressDto>().ReverseMap();
            CreateMap<EducationContactInformationModel, EducationContactInformationDto>().ReverseMap();
            CreateMap<LoginModel, LoginDto>();
            CreateMap<EducationListDto,EducationListModel>();
            CreateMap<CategoryAttributeListDto, CategoryAttributeListModel>().ForMember(dest=> dest.AttributeListModel,opt=> opt.MapFrom(src=>src.AttributeListDto));
            CreateMap<AttributeDto, AttributeModel>();
            CreateMap<CategoryDto, CategoryModel>();
            CreateMap<CityDto, CityModel>();
            CreateMap<DistrictDto, DistrictModel>();
            CreateMap<AddressDto, AddressModel>().ForMember(dest => dest.DistrictListModel, opt => opt.MapFrom(src => src.DistrictListDto)).ForMember(dest => dest.CityModel, opt => opt.MapFrom(src => src.CityDto));
            CreateMap<RegisterModel, RegisterDto>();
            CreateMap<EducationUpdateDto, EducationUpdateModel>().ReverseMap();
            CreateMap<EducationDetailDto, EducationDetailModel>();
            CreateMap<ImageDto, ImageModel>().ReverseMap();
            CreateMap<EducationAddressDetailDto, EducationAddressDetailModel>();
        }
    }
}
