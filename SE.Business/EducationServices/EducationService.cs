using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SE.Business.Helpers;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Data;

namespace SE.Business.EducationServices
{
    public class EducationService : IEducationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<EducationInsertDto> _educationInsertDtoValidator;
        private readonly IValidator<EducationUpdateDto> _educationUpdateDtoValidator;

        public EducationService(IUnitOfWork unitOfWork, IValidator<EducationInsertDto> educationInsertDtoValidator, IValidator<EducationUpdateDto> educationUpdateDtoValidator)
        {
            _unitOfWork = unitOfWork;
            _educationInsertDtoValidator = educationInsertDtoValidator;
            _educationUpdateDtoValidator = educationUpdateDtoValidator;
        }
        public int InsertEducation(EducationInsertDto educationInsertDto)
        {
            try
            {
                var educationInsertDtoValidate = _educationInsertDtoValidator.Validate(educationInsertDto, ruleSet: "all");
                if (!educationInsertDtoValidate.IsValid)
                {
                    throw new ValidationException(educationInsertDtoValidate.Errors);
                }

                var education = new Education
                {
                    Name = educationInsertDto.GeneralInformation.EducationName,
                    CategoryId = educationInsertDto.GeneralInformation.EducationType,
                    UserId = educationInsertDto.UserId,
                    Description = educationInsertDto.GeneralInformation.Description,
                    AuthorizedEmail = educationInsertDto.ContactInformation.AuthorizedEmail,
                    AuthorizedName = educationInsertDto.ContactInformation.AuthorizedName,
                    Email = educationInsertDto.ContactInformation.EducationEmail,
                    PhoneOne = educationInsertDto.ContactInformation.PhoneOne,
                    PhoneTwo = educationInsertDto.ContactInformation.PhoneTwo,
                    Website = educationInsertDto.ContactInformation.EducationWebsite
                };
                educationInsertDto.GeneralInformation.SeoUrl = UrlHelper.FriendlyUrl(educationInsertDto.GeneralInformation.EducationName);
                string seoUrl = _unitOfWork.EducationRepository.Table.Where(d => d.SeoUrl == educationInsertDto.GeneralInformation.SeoUrl).Select(d => d.SeoUrl).AsNoTracking().FirstOrDefault();

                if (seoUrl != null)
                {
                    education.SeoUrl = educationInsertDto.GeneralInformation.SeoUrl + Guid.NewGuid().ToString().Substring(0, 5);
                }
                else
                {
                    education.SeoUrl = educationInsertDto.GeneralInformation.SeoUrl;
                }
                var address = new EducationAddress
                {
                    AddressOne = educationInsertDto.AddressInformation.Address,
                    CityId = educationInsertDto.AddressInformation.CityId,
                    DistrictId = educationInsertDto.AddressInformation.DistrictId,
                };
                education.EducationAddress = address;
                foreach (var image in educationInsertDto.Images)
                {
                    education.Images.Add(new Image
                    {
                        ImageUrl = image,
                        Title = educationInsertDto.GeneralInformation.EducationName,
                        FirstVisible = false
                    });
                }
                foreach (var questionItem in educationInsertDto.Questions)
                {
                    education.Questions.Add(new Question
                    {
                        Title = questionItem.Question,
                        Answer = questionItem.Answer
                    });

                }
                foreach (var attributeId in educationInsertDto.Attributes)
                {
                    education.AttributeEducations.Add(new AttributeEducation
                    {
                        AttributeId = attributeId,
                        Education = education
                    });
                }
                _unitOfWork.EducationRepository.Insert(education);
                _unitOfWork.SaveChanges();
                return education.Id;

            }
            catch
            {
                throw;
            }
        }
        public List<EducationListDto> GetAllEducationListByUserId(string userId)
        {
            var educationListDto = (from e in _unitOfWork.EducationRepository.Table
                                    join c in _unitOfWork.CategoryRepository.Table on e.CategoryId equals c.Id
                                    join a in _unitOfWork.AddressRepository.Table on e.EducationAddress.Id equals a.Id
                                    join d in _unitOfWork.DistrictRepository.Table on a.DistrictId equals d.Id
                                    let image = (from i in _unitOfWork.ImageRepository.Table where (i.EducationId == e.Id && i.FirstVisible == true) select i.ImageUrl).FirstOrDefault()
                                    where e.UserId == userId
                                    select new EducationListDto
                                    {
                                        Id = e.Id,
                                        Name = e.Name,
                                        CategoryName = c.Name,
                                        CategorySeoUrl = c.SeoUrl,
                                        DistrictName = d.Name,
                                        Address = a.AddressOne,
                                        ImageUrl = image,
                                        SeoUrl = e.SeoUrl
                                    }).AsNoTracking().ToList();
            return educationListDto;

        }
        public EducationUpdateDto GetEducationUpdateDtoBySeoUrl(string seoUrl, string userId)
        {
            try
            {
                var education = _unitOfWork.EducationRepository.Include(a => a.AttributeEducations, e => e.EducationAddress, i => i.Images, q => q.Questions).Where(d => d.SeoUrl == seoUrl && d.UserId == userId).AsNoTracking().FirstOrDefault();
                if (education != null)
                {
                    var educationUpdateDto = new EducationUpdateDto();
                    educationUpdateDto.GeneralInformation.Id = education.Id;
                    educationUpdateDto.GeneralInformation.SeoUrl = education.SeoUrl;
                    educationUpdateDto.UserId = education.UserId;
                    educationUpdateDto.GeneralInformation.Description = education.Description;
                    educationUpdateDto.GeneralInformation.EducationName = education.Name;
                    educationUpdateDto.GeneralInformation.EducationType = education.CategoryId;
                    educationUpdateDto.AddressInformation.Address = education.EducationAddress.AddressOne;
                    educationUpdateDto.AddressInformation.CityId = education.EducationAddress.CityId;
                    educationUpdateDto.AddressInformation.DistrictId = education.EducationAddress.DistrictId;
                    educationUpdateDto.ContactInformation.AuthorizedEmail = education.AuthorizedEmail;
                    educationUpdateDto.ContactInformation.AuthorizedName = education.AuthorizedName;
                    educationUpdateDto.ContactInformation.EducationEmail = education.Email;
                    educationUpdateDto.ContactInformation.EducationWebsite = education.Website;
                    educationUpdateDto.ContactInformation.PhoneOne = education.PhoneOne;
                    educationUpdateDto.ContactInformation.PhoneTwo = education.PhoneTwo;
                    educationUpdateDto.Attributes = education.AttributeEducations.Select(d => d.AttributeId).ToArray();
                    educationUpdateDto.Images = education.Images.Select(d => d.ImageUrl).ToArray();
                    educationUpdateDto.Questions = education.Questions.Select(d => new EducationQuestionDto
                    {
                        Question = d.Title,
                        Answer = d.Answer
                    }).ToList();

                    return educationUpdateDto;
                }
                else
                {
                    throw new ArgumentNullException();
                }

            }
            catch
            {
                throw;
            }

        }
        public EducationDetailDto GetEducationDetailDtoBySeoUrl(string seoUrl)
        {
            try
            {
                var education = _unitOfWork.EducationRepository.Include(a => a.AttributeEducations, e => e.EducationAddress, i => i.Images, q => q.Questions, c =>c.EducationAddress.City, d=>d.EducationAddress.District).Where(d => d.SeoUrl == seoUrl).AsNoTracking().FirstOrDefault();
                if (education != null)
                {
                    var educationUpdateDto = new EducationDetailDto();
                    educationUpdateDto.GeneralInformation.Id = education.Id;
                    educationUpdateDto.GeneralInformation.SeoUrl = education.SeoUrl;
                    educationUpdateDto.UserId = education.UserId;
                    educationUpdateDto.GeneralInformation.Description = education.Description;
                    educationUpdateDto.GeneralInformation.EducationName = education.Name;
                    educationUpdateDto.GeneralInformation.EducationType = education.CategoryId;
                    educationUpdateDto.AddressInformation.Address = education.EducationAddress.AddressOne;
                    educationUpdateDto.AddressInformation.CityName = education.EducationAddress.City.Name;
                    educationUpdateDto.AddressInformation.DistrictName = education.EducationAddress.District.Name;
                    educationUpdateDto.ContactInformation.AuthorizedEmail = education.AuthorizedEmail;
                    educationUpdateDto.ContactInformation.AuthorizedName = education.AuthorizedName;
                    educationUpdateDto.ContactInformation.EducationEmail = education.Email;
                    educationUpdateDto.ContactInformation.EducationWebsite = education.Website;
                    educationUpdateDto.ContactInformation.PhoneOne = education.PhoneOne;
                    educationUpdateDto.ContactInformation.PhoneTwo = education.PhoneTwo;
                    var selectedAttributeIds = education.AttributeEducations.Select(d => d.AttributeId).ToArray();
                    var attributeList = _unitOfWork.AttributeRepository.Table.Where(d => selectedAttributeIds.Contains(d.Id));

                    educationUpdateDto.CategoryAttributeList = attributeList.AsEnumerable().GroupBy(d => d.AttributeCategoryId).Select(d => new CategoryAttributeListDto
                    {
                        CategoryName = _unitOfWork.AttributeCategoryRepository.Table.Where(x => x.Id == d.Key).FirstOrDefault().Name,
                        AttributeListDto = d.Select(x => new AttributeDto
                        {
                            Id = x.Id,
                            Name = x.Name
                        }).ToList()
                    }).ToList();
                    educationUpdateDto.Images = education.Images.Select(d => d.ImageUrl).ToArray();
                    educationUpdateDto.Questions = education.Questions.Select(d => new EducationQuestionDto
                    {
                        Question = d.Title,
                        Answer = d.Answer
                    }).ToList();

                    return educationUpdateDto;
                }
                else
                {
                    throw new ArgumentNullException();
                }

            }
            catch
            {
                throw;
            }
        }

        public int UpdateEducation(EducationUpdateDto educationUpdateDto)
        {
            try
            {
                var educationUpdateDtoValidate = _educationUpdateDtoValidator.Validate(educationUpdateDto, ruleSet: "all");
                if (!educationUpdateDtoValidate.IsValid)
                {
                    throw new ValidationException(educationUpdateDtoValidate.Errors);
                }
                var education = _unitOfWork.EducationRepository.Include(a => a.AttributeEducations, e => e.EducationAddress, i => i.Images, q => q.Questions).Where(d => d.Id == educationUpdateDto.GeneralInformation.Id && d.UserId == educationUpdateDto.UserId).FirstOrDefault();
                if (education != null)
                {
                    education.Name = educationUpdateDto.GeneralInformation.EducationName;
                    education.Description = educationUpdateDto.GeneralInformation.Description;
                    education.CategoryId = educationUpdateDto.GeneralInformation.EducationType;

                    var seoUrl = UrlHelper.FriendlyUrl(educationUpdateDto.GeneralInformation.EducationName);
                    if (seoUrl != education.SeoUrl)
                    {
                        string savedSeoUrl = _unitOfWork.EducationRepository.Table.Where(d => d.SeoUrl == seoUrl).Select(d => d.SeoUrl).FirstOrDefault();

                        if (savedSeoUrl != null)
                        {
                            education.SeoUrl = seoUrl + "-2";
                        }
                        else
                        {
                            education.SeoUrl = seoUrl;
                        }
                    }


                    education.AuthorizedEmail = educationUpdateDto.ContactInformation.AuthorizedEmail;
                    education.AuthorizedName = educationUpdateDto.ContactInformation.AuthorizedName;
                    education.Email = educationUpdateDto.ContactInformation.EducationEmail;
                    education.Website = educationUpdateDto.ContactInformation.EducationWebsite;
                    education.PhoneOne = educationUpdateDto.ContactInformation.PhoneOne;
                    education.PhoneTwo = educationUpdateDto.ContactInformation.PhoneTwo;

                    education.EducationAddress.AddressOne = educationUpdateDto.AddressInformation.Address;
                    education.EducationAddress.CityId = educationUpdateDto.AddressInformation.CityId;
                    education.EducationAddress.DistrictId = educationUpdateDto.AddressInformation.DistrictId;

                    foreach (var imageUrl in educationUpdateDto.Images)
                    {
                        education.Images.Add(new Image
                        {
                            ImageUrl = imageUrl,
                            Title = educationUpdateDto.GeneralInformation.EducationName,
                        });
                    }
                    education.AttributeEducations.Clear();
                    education.Questions.Clear();

                    foreach (var questionItem in educationUpdateDto.Questions)
                    {
                        education.Questions.Add(new Question
                        {
                            Title = questionItem.Question,
                            Answer = questionItem.Answer
                        });

                    }
                    foreach (var attributeId in educationUpdateDto.Attributes)
                    {
                        education.AttributeEducations.Add(new AttributeEducation
                        {
                            AttributeId = attributeId,
                            Education = education
                        });
                    }
                    _unitOfWork.EducationRepository.Update(education);
                    _unitOfWork.SaveChanges();
                    return education.Id;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteEducation(int educationId, string userId)
        {

            try
            {
                var education = _unitOfWork.EducationRepository.Include(a => a.AttributeEducations, e => e.EducationAddress, i => i.Images, q => q.Questions).Where(d => d.Id == educationId && d.UserId == userId).AsNoTracking().FirstOrDefault();
                if (education != null)
                {
                    _unitOfWork.EducationRepository.Delete(education);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    throw new ArgumentNullException();
                }

            }
            catch
            {
                throw;
            }

        }

        public List<EducationListDto> GetAllEducationListByCategoryId(int categoryId)
        {
            var educationListDto = (from e in _unitOfWork.EducationRepository.Table
                                    join c in _unitOfWork.CategoryRepository.Table on e.CategoryId equals c.Id
                                    join a in _unitOfWork.AddressRepository.Table on e.EducationAddress.Id equals a.Id
                                    join d in _unitOfWork.DistrictRepository.Table on a.DistrictId equals d.Id
                                    let image = (from i in _unitOfWork.ImageRepository.Table where (i.EducationId == e.Id && i.FirstVisible==true) select i.ImageUrl).FirstOrDefault()
                                    where c.Id == categoryId
                                    select new EducationListDto
                                    {
                                        Id = e.Id,
                                        Name = e.Name,
                                        CategoryName = c.Name,
                                        CategorySeoUrl = c.SeoUrl,
                                        DistrictName = d.Name,
                                        Address = a.AddressOne,
                                        ImageUrl = image,
                                        SeoUrl = e.SeoUrl
                                    }).AsNoTracking().ToList();
            return educationListDto;
        }

        public List<ImageDto> GetAllEducationImageDtoByEducationId(int educationId)
        {
            try
            {
                var images = _unitOfWork.ImageRepository.Table.Where(d => d.EducationId == educationId).Select(
                    d => new ImageDto
                    {
                        Id = d.Id,
                        ImageUrl = d.ImageUrl,
                        FirstVisible = d.FirstVisible,
                        Title = d.Title,
                        EducationId = d.EducationId
                    }).ToList();
                return images;
            }
            catch
            {
                throw;
            }
        }

        public void InsertFirstVisibleImage(ImageDto imageDto)
        {
            try
            {
                var image = _unitOfWork.ImageRepository.Table.Where(d => d.Id == imageDto.Id && d.EducationId==imageDto.EducationId).FirstOrDefault();
                image.FirstVisible = true;
                _unitOfWork.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void UpdateFirstVisibleImage(ImageDto imageDto)
        {
            try
            {
                var images = _unitOfWork.ImageRepository.Table.Where(d => d.EducationId == imageDto.EducationId).ToList();
                foreach (var image in images)
                {
                    if (image.Id == imageDto.Id)
                    {
                        image.FirstVisible = true;
                        break;
                    }
                    image.FirstVisible = false;
                }
                _unitOfWork.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
