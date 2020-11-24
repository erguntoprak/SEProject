using AutoMapper;
using SE.Core.DTO;
using SE.Web.Model;
using SE.Web.Model.Account;
using SE.Web.Model.Address;
using SE.Web.Model.Attribute;
using SE.Web.Model.Blog;
using SE.Web.Model.Category;
using SE.Web.Model.Common;
using SE.Web.Model.Education;
using SE.Web.Model.Image;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Infrastructure.AutoMapper
{
    public class AutoMapping : Profile
    {
        DateTimeFormatInfo fmt = (new CultureInfo("tr-TR")).DateTimeFormat;
        public AutoMapping()
        {
            CreateMap<EducationInsertModel, EducationInsertDto>();
            CreateMap<EducationGeneralInformationModel, EducationGeneralInformationDto>().ReverseMap(); ;
            CreateMap<EducationQuestionModel, EducationQuestionDto>().ReverseMap();
            CreateMap<EducationAddressModel, EducationAddressDto>().ReverseMap();
            CreateMap<EducationContactInformationModel, EducationContactInformationDto>().ReverseMap();
            CreateMap<LoginModel, LoginDto>();
            CreateMap<EducationListDto, EducationListModel>();
            CreateMap<CategoryAttributeListDto, CategoryAttributeListModel>().ForMember(dest => dest.AttributeListModel, opt => opt.MapFrom(src => src.AttributeListDto));
            CreateMap<AttributeDto, AttributeModel>();
            CreateMap<CategoryDto, CategoryModel>().ReverseMap();
            CreateMap<CityDto, CityModel>();
            CreateMap<DistrictDto, DistrictModel>();
            CreateMap<AddressDto, AddressModel>().ForMember(dest => dest.DistrictListModel, opt => opt.MapFrom(src => src.DistrictListDto)).ForMember(dest => dest.CityModel, opt => opt.MapFrom(src => src.CityDto));
            CreateMap<RegisterModel, RegisterDto>();
            CreateMap<EducationUpdateDto, EducationUpdateModel>().ReverseMap();
            CreateMap<EducationDetailDto, EducationDetailModel>();
            CreateMap<ImageDto, ImageModel>().ReverseMap();
            CreateMap<EducationAddressDetailDto, EducationAddressDetailModel>();
            CreateMap<EducationContactFormInsertModel, EducationContactFormInsertDto>();
            CreateMap<EducationContactFormListDto, EducationContactFormListModel>().ForMember(dest => dest.CreateDateTime, opt => opt.MapFrom(src => src.CreateDateTime.ToString("d", fmt)));
            CreateMap<BlogInsertModel, BlogInsertDto>();
            CreateMap<BlogItemModel, BlogItemDto>().ReverseMap();
            CreateMap<BlogListDto, BlogListModel>().ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreateTime.ToString("D", fmt)));
            CreateMap<BlogDetailDto, BlogDetailModel>().ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreateTime.ToString("D", fmt)));
            CreateMap<BlogUpdateModel, BlogUpdateDto>().ReverseMap();
            CreateMap<SocialInformationModel, SocialInformationDto>();
            CreateMap<SocialInformationDto, SocialInformationModel>();
            CreateMap<SearchEducationDto, SearchEducationModel>();
            CreateMap<FilterModel, FilterDto>();
            CreateMap<EducationFilterListDto, EducationFilterListModel>();
            CreateMap<EmailConfirmation, EmailConfirmationDto>();
            CreateMap<UserListDto, UserListModel>();
            CreateMap<RoleDto, RoleModel>();
            CreateMap<UserDto, UserModel>();
            CreateMap<UserUpdateModel, UserUpdateDto>();
            CreateMap<UserPasswordUpdateModel, UserPasswordUpdateDto>();
            CreateMap<ResetPasswordModel, ResetPasswordDto>();
            CreateMap<AttributeCategoryModel, AttributeCategoryDto>().ReverseMap();
            CreateMap<CategoryAttributeCategoryModel, CategoryAttributeCategoryDto>();
            CreateMap<CategoryAttributeCategoryInsertModel, CategoryAttributeCategoryInsertDto>();
            CreateMap<AttributeListDto, AttributeListModel>();
            CreateMap<AttributeDto, AttributeModel>().ReverseMap();
            CreateMap<DashboardDataDto, DashboardDataModel>();
            CreateMap<DashboardFilterModel, DashboardFilterDto>();
            CreateMap<EducationUploadImageModel, EducationUploadImageDto>().ReverseMap();
        }
    }
}
