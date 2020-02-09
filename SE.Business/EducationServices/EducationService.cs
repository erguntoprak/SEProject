using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Data;

namespace SE.Business.EducationServices
{
    public class EducationService : IEducationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EducationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void InsertEducation(EducationInsertDto educationInsertDto)
        {
            try
            {
                var education = new Education
                {
                    Name = educationInsertDto.GeneralInformation.EducationName,
                    CategoryId = educationInsertDto.GeneralInformation.EducationType,
                    UserId = educationInsertDto.GeneralInformation.UserId,
                    Description = educationInsertDto.GeneralInformation.Description,
                    AuthorizedEmail = educationInsertDto.ContactInformation.AuthorizedEmail,
                    AuthorizedName = educationInsertDto.ContactInformation.AuthorizedName,
                    Email = educationInsertDto.ContactInformation.EducationEmail,
                    PhoneOne = educationInsertDto.ContactInformation.PhoneOne,
                    PhoneTwo = educationInsertDto.ContactInformation.PhoneTwo,
                    Website = educationInsertDto.ContactInformation.EducationWebsite
                };
                string seoUrl = _unitOfWork.EducationRepository.Table.Where(d => d.SeoUrl == educationInsertDto.GeneralInformation.SeoUrl).Select(d => d.SeoUrl).FirstOrDefault();

                if (seoUrl != null)
                {
                    education.SeoUrl = educationInsertDto.GeneralInformation.SeoUrl + "-2";
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
                foreach(var image in educationInsertDto.Images)
                {
                    education.Images.Add(new Image
                    {
                        ImageBase64 = image,
                        Title = educationInsertDto.GeneralInformation.EducationName,
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
                                    let image = (from i in _unitOfWork.ImageRepository.Table where      i.EducationId==e.Id select i.ImageBase64).FirstOrDefault()
                                    where e.UserId == userId
                                    select new EducationListDto
                                    {
                                        Name=e.Name,
                                        CategoryName = c.Name,
                                        CategorySeoUrl = c.SeoUrl,
                                        DistrictName = d.Name,
                                        Address = a.AddressOne,
                                        Base64Image = image,
                                        SeoUrl = e.SeoUrl
                                    }).ToList();
            return educationListDto;
                                   
        }
    }
}
