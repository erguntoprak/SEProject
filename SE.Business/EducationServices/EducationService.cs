using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LazZiya.ImageResize;
using Microsoft.EntityFrameworkCore;
using SE.Business.Constants;
using SE.Business.Helpers;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Core.Utilities.Results;
using SE.Data;
using SixLabors.ImageSharp.Processing;
using SE.Core.Aspects.Autofac.Validation;
using SE.Business.Infrastructure.FluentValidation.Validations;
using SE.Core.Aspects.Autofac.Caching;
using SixLabors.ImageSharp;
using SE.Core.Utilities.Security.Http;

namespace SE.Business.EducationServices
{
    public class EducationService : IEducationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string[] acceptFileType = { "image/png", "image/jpeg" };
        private readonly IRequestContext _requestContext;

        public EducationService(IUnitOfWork unitOfWork, IRequestContext requestContext)
        {
            _unitOfWork = unitOfWork;
            _requestContext = requestContext;
        }

        [ValidationAspect(typeof(EducationInsertDtoValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<int>> InsertEducationAsync(EducationInsertDto educationInsertDto)
        {
            var education = new Education
            {
                Name = educationInsertDto.GeneralInformation.EducationName,
                CategoryId = educationInsertDto.GeneralInformation.EducationType,
                UserId = _requestContext.UserId,
                Description = educationInsertDto.GeneralInformation.Description,
                AuthorizedEmail = educationInsertDto.ContactInformation.AuthorizedEmail,
                AuthorizedName = educationInsertDto.ContactInformation.AuthorizedName,
                Email = educationInsertDto.ContactInformation.EducationEmail,
                PhoneOne = educationInsertDto.ContactInformation.PhoneOne,
                PhoneTwo = educationInsertDto.ContactInformation.PhoneTwo,
                Website = educationInsertDto.ContactInformation.EducationWebsite,
                YoutubeVideoOne = educationInsertDto.SocialInformation.YoutubeVideoOne,
                YoutubeVideoTwo = educationInsertDto.SocialInformation.YoutubeVideoTwo,
                YoutubeAccountUrl = educationInsertDto.SocialInformation.YoutubeAccountUrl,
                TwitterAccountUrl = educationInsertDto.SocialInformation.TwitterAccountUrl,
                InstagramAccountUrl = educationInsertDto.SocialInformation.InstagramAccountUrl,
                FacebookAccountUrl = educationInsertDto.SocialInformation.FacebookAccountUrl,
                MapCode = educationInsertDto.SocialInformation.MapCode,
                IsActive = false
            };
            educationInsertDto.GeneralInformation.SeoUrl = UrlHelper.FriendlyUrl(educationInsertDto.GeneralInformation.EducationName);
            string seoUrl = await _unitOfWork.EducationRepository.Table.Select(d => d.SeoUrl).AsNoTracking().FirstOrDefaultAsync(d => d == educationInsertDto.GeneralInformation.SeoUrl);

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

            foreach (var questionItem in educationInsertDto.Questions)
            {
                if (string.IsNullOrEmpty(questionItem.Answer) || string.IsNullOrEmpty(questionItem.Question))
                {
                    education.Questions.Add(new Question
                    {
                        Title = questionItem.Question,
                        Answer = questionItem.Answer
                    });
                }
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
            await _unitOfWork.SaveChangesAsync();

            return new SuccessDataResult<int>(education.Id);
        }
        public async Task<IDataResult<IEnumerable<EducationListDto>>> GetAllEducationListByUserIdAsync()
        {

            var educationListDto = await (from e in _unitOfWork.EducationRepository.Table
                                          join c in _unitOfWork.CategoryRepository.Table on e.CategoryId equals c.Id
                                          join a in _unitOfWork.AddressRepository.Table on e.EducationAddress.Id equals a.Id
                                          join d in _unitOfWork.DistrictRepository.Table on a.DistrictId equals d.Id
                                          let image = (from i in _unitOfWork.ImageRepository.Table where (i.EducationId == e.Id && i.FirstVisible == true) select i.ImageUrl).FirstOrDefault()
                                          where e.UserId == _requestContext.UserId
                                          select new EducationListDto
                                          {
                                              Id = e.Id,
                                              Name = e.Name,
                                              CategoryId = e.CategoryId,
                                              CategoryName = c.Name,
                                              CategorySeoUrl = c.SeoUrl,
                                              DistrictName = d.Name,
                                              DistrictSeoUrl = d.SeoUrl,
                                              DistrictId = d.Id,
                                              Address = a.AddressOne,
                                              IsActive = e.IsActive,
                                              ImageUrl = image,
                                              SeoUrl = e.SeoUrl
                                          }).AsNoTracking().ToListAsync();

            return new SuccessDataResult<IEnumerable<EducationListDto>>(educationListDto);
        }

        [CacheAspect]
        public async Task<IDataResult<IEnumerable<EducationListDto>>> GetAllEducationListAsync()
        {
            var educationListDto = await (from e in _unitOfWork.EducationRepository.Table
                                          join c in _unitOfWork.CategoryRepository.Table on e.CategoryId equals c.Id
                                          join a in _unitOfWork.AddressRepository.Table on e.EducationAddress.Id equals a.Id
                                          join d in _unitOfWork.DistrictRepository.Table on a.DistrictId equals d.Id
                                          select new EducationListDto
                                          {
                                              Id = e.Id,
                                              Name = e.Name,
                                              IsActive = e.IsActive,
                                              CategoryId = e.CategoryId,
                                              CategoryName = c.Name,
                                              CategorySeoUrl = c.SeoUrl,
                                              DistrictName = d.Name,
                                              DistrictSeoUrl = d.SeoUrl,
                                              DistrictId = d.Id,
                                              Address = a.AddressOne,
                                              SeoUrl = e.SeoUrl
                                          }).AsNoTracking().ToListAsync();

            return new SuccessDataResult<IEnumerable<EducationListDto>>(educationListDto);
        }
        public async Task<IDataResult<EducationUpdateDto>> GetEducationUpdateDtoBySeoUrlAsync(string seoUrl)
        {
            var education = await _unitOfWork.EducationRepository.Include(a => a.AttributeEducations, e => e.EducationAddress, i => i.Images, q => q.Questions).AsNoTracking().FirstOrDefaultAsync(d => d.SeoUrl == seoUrl && (_requestContext.Roles.Contains(GeneralConstants.Admin) ? true : d.UserId == _requestContext.UserId));

            if (education == null)
                return new ErrorDataResult<EducationUpdateDto>(Messages.ObjectIsNull);


            var educationUpdateDto = new EducationUpdateDto();
            educationUpdateDto.GeneralInformation.Id = education.Id;
            educationUpdateDto.GeneralInformation.SeoUrl = education.SeoUrl;
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

            educationUpdateDto.SocialInformation.InstagramAccountUrl = education.InstagramAccountUrl;
            educationUpdateDto.SocialInformation.MapCode = education.MapCode;
            educationUpdateDto.SocialInformation.TwitterAccountUrl = education.TwitterAccountUrl;
            educationUpdateDto.SocialInformation.YoutubeAccountUrl = education.YoutubeAccountUrl;
            educationUpdateDto.SocialInformation.YoutubeVideoOne = education.YoutubeVideoOne;
            educationUpdateDto.SocialInformation.YoutubeVideoTwo = education.YoutubeVideoTwo;
            educationUpdateDto.SocialInformation.FacebookAccountUrl = education.FacebookAccountUrl;

            return new SuccessDataResult<EducationUpdateDto>(educationUpdateDto);

        }
        public async Task<IDataResult<EducationDetailDto>> GetEducationDetailDtoBySeoUrlAsync(string seoUrl)
        {
            var education = await _unitOfWork.EducationRepository.Include(x => x.Category, a => a.AttributeEducations, e => e.EducationAddress, i => i.Images, q => q.Questions, c => c.EducationAddress.City, d => d.EducationAddress.District).AsNoTracking().FirstOrDefaultAsync(d => d.SeoUrl == seoUrl && d.IsActive == true);

            if (education == null)
                return new ErrorDataResult<EducationDetailDto>(Messages.ObjectIsNull);

            var educationDetailDto = new EducationDetailDto();
            var blogList = await _unitOfWork.BlogRepository.Include(d => d.User).Where(d => d.UserId == education.UserId && d.IsActive).OrderByDescending(d => d.UpdateTime).Take(3).AsNoTracking().Select(d => new BlogListDto
            {
                Id = d.Id,
                CreateTime = d.CreateTime,
                UserSeoUrl = UrlHelper.FriendlyUrl(d.User.UserName),
                UserName = d.User.UserName,
                FirstVisibleImageName = d.FirstVisibleImageName,
                Title = d.Title,
                SeoUrl = d.SeoUrl
            }).ToListAsync();

            educationDetailDto.BlogList = blogList;

            educationDetailDto.GeneralInformation.Id = education.Id;
            educationDetailDto.GeneralInformation.SeoUrl = education.SeoUrl;
            educationDetailDto.GeneralInformation.Description = education.Description;
            educationDetailDto.GeneralInformation.EducationName = education.Name;
            educationDetailDto.GeneralInformation.EducationType = education.CategoryId;
            educationDetailDto.GeneralInformation.CategoryName = education.Category.Name;
            educationDetailDto.GeneralInformation.CategorySeoUrl = education.Category.SeoUrl;


            educationDetailDto.AddressInformation.Address = education.EducationAddress.AddressOne;
            educationDetailDto.AddressInformation.CityName = education.EducationAddress.City.Name;
            educationDetailDto.AddressInformation.DistrictName = education.EducationAddress.District.Name;
            educationDetailDto.AddressInformation.DistricSeoUrl = education.EducationAddress.District.SeoUrl;

            educationDetailDto.ContactInformation.AuthorizedEmail = education.AuthorizedEmail;
            educationDetailDto.ContactInformation.AuthorizedName = education.AuthorizedName;
            educationDetailDto.ContactInformation.EducationEmail = education.Email;
            educationDetailDto.ContactInformation.EducationWebsite = education.Website;
            educationDetailDto.ContactInformation.PhoneOne = education.PhoneOne;
            educationDetailDto.ContactInformation.PhoneTwo = education.PhoneTwo;
            var selectedAttributeIds = education.AttributeEducations.Select(d => d.AttributeId).ToArray();
            var attributeList = _unitOfWork.AttributeRepository.Table.Where(d => selectedAttributeIds.Contains(d.Id));

            educationDetailDto.CategoryAttributeList = attributeList.AsEnumerable().GroupBy(d => d.AttributeCategoryId).Select(d => new CategoryAttributeListDto
            {
                CategoryName = _unitOfWork.AttributeCategoryRepository.Table.Where(x => x.Id == d.Key).FirstOrDefault().Name,
                AttributeListDto = d.Select(x => new AttributeDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()
            }).ToList();
            educationDetailDto.Images = education.Images.Select(d => d.ImageUrl).ToArray();
            educationDetailDto.Questions = education.Questions.Select(d => new EducationQuestionDto
            {
                Question = d.Title,
                Answer = d.Answer
            }).ToList();

            educationDetailDto.SocialInformation.InstagramAccountUrl = education.InstagramAccountUrl;
            educationDetailDto.SocialInformation.MapCode = education.MapCode;
            educationDetailDto.SocialInformation.TwitterAccountUrl = education.TwitterAccountUrl;
            educationDetailDto.SocialInformation.YoutubeAccountUrl = education.YoutubeAccountUrl;
            educationDetailDto.SocialInformation.YoutubeVideoOne = education.YoutubeVideoOne;
            educationDetailDto.SocialInformation.YoutubeVideoTwo = education.YoutubeVideoTwo;
            educationDetailDto.SocialInformation.FacebookAccountUrl = education.FacebookAccountUrl;

            return new SuccessDataResult<EducationDetailDto>(educationDetailDto);

        }

        [ValidationAspect(typeof(EducationUpdateDtoValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<int>> UpdateEducationAsync(EducationUpdateDto educationUpdateDto)
        {
            var education = await _unitOfWork.EducationRepository.Include(a => a.AttributeEducations, e => e.EducationAddress, q => q.Questions).FirstOrDefaultAsync(d => d.Id == educationUpdateDto.GeneralInformation.Id && (_requestContext.Roles.Contains(GeneralConstants.Admin) ? true : d.UserId == _requestContext.UserId));

            if (education == null)
                return new ErrorDataResult<int>(Messages.ObjectIsNull);

            education.Name = educationUpdateDto.GeneralInformation.EducationName;
            education.Description = educationUpdateDto.GeneralInformation.Description;
            education.CategoryId = educationUpdateDto.GeneralInformation.EducationType;

            var seoUrl = UrlHelper.FriendlyUrl(educationUpdateDto.GeneralInformation.EducationName);
            if (seoUrl != education.SeoUrl)
            {
                string savedSeoUrl = _unitOfWork.EducationRepository.Table.Where(d => d.SeoUrl == seoUrl).Select(d => d.SeoUrl).FirstOrDefault();

                if (savedSeoUrl != null)
                {
                    education.SeoUrl = seoUrl + Guid.NewGuid();
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

            education.YoutubeAccountUrl = educationUpdateDto.SocialInformation.YoutubeAccountUrl;
            education.FacebookAccountUrl = educationUpdateDto.SocialInformation.FacebookAccountUrl;
            education.TwitterAccountUrl = educationUpdateDto.SocialInformation.TwitterAccountUrl;
            education.InstagramAccountUrl = educationUpdateDto.SocialInformation.InstagramAccountUrl;
            education.YoutubeVideoOne = educationUpdateDto.SocialInformation.YoutubeVideoOne;
            education.YoutubeVideoTwo = educationUpdateDto.SocialInformation.YoutubeVideoTwo;
            education.MapCode = educationUpdateDto.SocialInformation.MapCode;
            education.IsActive = false;

            education.AttributeEducations.Clear();
            education.Questions.Clear();

            foreach (var questionItem in educationUpdateDto.Questions)
            {
                if (!string.IsNullOrEmpty(questionItem.Answer) || !string.IsNullOrEmpty(questionItem.Question))
                {
                    education.Questions.Add(new Question
                    {
                        Title = questionItem.Question,
                        Answer = questionItem.Answer
                    });
                }
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
            await _unitOfWork.SaveChangesAsync();

            return new SuccessDataResult<int>(education.Id);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteEducationAsync(int educationId)
        {

            var education = await _unitOfWork.EducationRepository.Include(a => a.AttributeEducations, e => e.EducationAddress, i => i.Images, q => q.Questions).AsNoTracking().FirstOrDefaultAsync(d => d.Id == educationId && (_requestContext.Roles.Contains(GeneralConstants.Admin) ? true : d.UserId == _requestContext.UserId));

            if (education == null)
                return new ErrorResult(Messages.ObjectIsNull);

            _unitOfWork.EducationRepository.Delete(education);

            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult(Messages.Deleted);
        }

        public async Task<IDataResult<IEnumerable<EducationListDto>>> GetAllEducationListByCategoryIdAndDistrictIdAsync(int categoryId, int districtId)
        {
            var educationListDto = await (from e in _unitOfWork.EducationRepository.Table
                                          join c in _unitOfWork.CategoryRepository.Table on e.CategoryId equals c.Id
                                          join a in _unitOfWork.AddressRepository.Table on e.EducationAddress.Id equals a.Id
                                          join d in _unitOfWork.DistrictRepository.Table on a.DistrictId equals d.Id
                                          let image = (from i in _unitOfWork.ImageRepository.Table where (i.EducationId == e.Id && i.FirstVisible == true) select i.ImageUrl).FirstOrDefault()
                                          where (categoryId == 0 ? true : (c.Id == categoryId)) && (districtId == 0 ? true : (d.Id == districtId)) && e.IsActive == true
                                          select new EducationListDto
                                          {
                                              Id = e.Id,
                                              Name = e.Name,
                                              IsActive = e.IsActive,
                                              CategoryId = e.CategoryId,
                                              CategoryName = c.Name,
                                              CategorySeoUrl = c.SeoUrl,
                                              DistrictName = d.Name,
                                              DistrictSeoUrl = d.SeoUrl,
                                              DistrictId = d.Id,
                                              Address = a.AddressOne,
                                              ImageUrl = image,
                                              SeoUrl = e.SeoUrl
                                          }).AsNoTracking().ToArray().RandomListAsync(20);

            return new SuccessDataResult<IEnumerable<EducationListDto>>(educationListDto);
        }

        public async Task<IDataResult<IEnumerable<ImageDto>>> GetAllEducationImageDtoByEducationIdAsync(int educationId)
        {

            var educationImages = await _unitOfWork.ImageRepository.Table.Where(d => d.EducationId == educationId).Select(
                d => new ImageDto
                {
                    Id = d.Id,
                    ImageUrl = d.ImageUrl,
                    FirstVisible = d.FirstVisible,
                    Title = d.Title,
                    EducationId = d.EducationId
                }).ToListAsync();

            return new SuccessDataResult<IEnumerable<ImageDto>>(educationImages);
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> InsertFirstVisibleImageAsync(ImageDto imageDto)
        {

            var image = await _unitOfWork.ImageRepository.Table.FirstOrDefaultAsync(d => d.Id == imageDto.Id && d.EducationId == imageDto.EducationId);

            if (image == null)
                return new ErrorResult(Messages.ObjectIsNull);

            image.FirstVisible = true;
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult(Messages.Added);
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> UpdateFirstVisibleImageAsync(ImageDto imageDto)
        {
            var images = await _unitOfWork.ImageRepository.Table.Where(d => d.EducationId == imageDto.EducationId).ToListAsync();
            foreach (var image in images)
            {
                if (image.Id == imageDto.Id)
                {
                    image.FirstVisible = true;
                    break;
                }
                image.FirstVisible = false;
            }
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult(Messages.Updated);
        }

        [ValidationAspect(typeof(EducationContactFormInsertDtoValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IResult> InsertEducationContactFormAsync(EducationContactFormInsertDto educationContactFormDto)
        {

            _unitOfWork.EducationContactFormRepository.Insert(new EducationContactForm
            {
                NameSurname = educationContactFormDto.NameSurname,
                Email = educationContactFormDto.Email,
                PhoneNumber = educationContactFormDto.PhoneNumber,
                EducationId = educationContactFormDto.EducationId,
                CreateDateTime = educationContactFormDto.CreateDateTime
            });
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult(Messages.Updated);

        }

        public async Task<IDataResult<IEnumerable<EducationContactFormListDto>>> GetEducationContactFormListDtoBySeoUrlAsync(string seoUrl)
        {

            var education = await _unitOfWork.EducationRepository.Include(f => f.EducationContactForms).AsNoTracking().FirstOrDefaultAsync(d => d.SeoUrl == seoUrl && (_requestContext.Roles.Contains(GeneralConstants.Admin) ? true : d.UserId == _requestContext.UserId));

            if (education == null)
                return new ErrorDataResult<IEnumerable<EducationContactFormListDto>>(Messages.ObjectIsNull);


            List<EducationContactFormListDto> educationContactFormListDto = new List<EducationContactFormListDto>();
            if (education.EducationContactForms != null)
            {
                educationContactFormListDto = education.EducationContactForms.Select(d => new EducationContactFormListDto
                {
                    NameSurname = d.NameSurname,
                    Email = d.Email,
                    PhoneNumber = d.PhoneNumber,
                    CreateDateTime = d.CreateDateTime
                }).ToList();
            }
            return new SuccessDataResult<IEnumerable<EducationContactFormListDto>>(educationContactFormListDto);
        }

        [CacheAspect]
        public async Task<IDataResult<IEnumerable<SearchEducationDto>>> GetAllSearchEducationListAsync()
        {
            var searchEducationList = await (from e in _unitOfWork.EducationRepository.Table
                                             join c in _unitOfWork.CategoryRepository.Table on e.CategoryId equals c.Id
                                             join a in _unitOfWork.AddressRepository.Table on e.EducationAddress.Id equals a.Id
                                             join d in _unitOfWork.DistrictRepository.Table on a.DistrictId equals d.Id
                                             select new SearchEducationDto
                                             {
                                                 Name = e.Name,
                                                 SeoUrl = UrlHelper.FriendlyUrl(d.Name) + "/" + c.SeoUrl + '/' + e.SeoUrl
                                             }).ToListAsync();

            return new SuccessDataResult<IEnumerable<SearchEducationDto>>(searchEducationList);
        }

        public async Task<IDataResult<IEnumerable<EducationFilterListDto>>> GetAllEducationListByFilterAsync(FilterDto filterDto)
        {
            var educationListDtoQuery = _unitOfWork.EducationRepository.Include(c => c.Category, ea => ea.EducationAddress, eat => eat.AttributeEducations, i => i.Images, d => d.EducationAddress.District).Where(d => d.Category.Id == filterDto.CategoryId && d.EducationAddress.District.Id == filterDto.DistrictId && d.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(filterDto.SearchText))
            {
                educationListDtoQuery = educationListDtoQuery.Where(d => d.Name.ToLower().Contains(filterDto.SearchText.ToLower()));
            }
            var educationListDto = await educationListDtoQuery.Select(d => new EducationFilterListDto
            {
                Id = d.Id,
                Name = d.Name,
                CategoryId = d.Category.Id,
                CategoryName = d.Category.Name,
                CategorySeoUrl = d.Category.SeoUrl,
                DistrictName = d.EducationAddress.District.Name,
                DistrictSeoUrl = d.EducationAddress.District.SeoUrl,
                DistrictId = d.EducationAddress.District.Id,
                Address = d.EducationAddress.AddressOne,
                ImageUrl = d.Images.Where(i => i.FirstVisible).FirstOrDefault().ImageUrl,
                SeoUrl = d.SeoUrl,
                AttributeIds = d.AttributeEducations.Select(at => at.AttributeId).ToArray()
            }).AsNoTracking().ToArrayAsync();

            var randomEducationListDto = await educationListDto.RandomListAsync(educationListDto.Length);

            return new SuccessDataResult<IEnumerable<EducationFilterListDto>>(randomEducationListDto);
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> UploadEducationImageAsync(EducationUploadImageDto educationUploadImageDto)
        {
            var education = await _unitOfWork.EducationRepository.GetByIdAsync(educationUploadImageDto.EducationId);

            foreach (var file in educationUploadImageDto.UploadImages)
            {
                if (acceptFileType.Contains(file.ContentType))
                {
                    string imageName = $"{UrlHelper.FriendlyUrl(education.Name)}_{Path.GetRandomFileName()}";
                    var fullPath = Path.Combine(educationUploadImageDto.Path, imageName);

                    if (file.Length > 0)
                    {
                        using (var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream()))
                        {
                            var options = new ResizeOptions
                            {
                                Mode = ResizeMode.Crop,
                                Position = AnchorPositionMode.Center,
                                Size = new Size(1000, 600)
                            };
                            image.Mutate(d => d.Resize(options));
                            string bigFile = Path.Combine(fullPath + "_1000x600.jpg");
                            await image.SaveAsync(bigFile);
                        }
                    }

                    _unitOfWork.ImageRepository.Insert(new Core.Entities.Image { ImageUrl = imageName, Title = education.Name, FirstVisible = false, EducationId = education.Id });
                }
            }
            _unitOfWork.SaveChanges();
            return new SuccessResult(Messages.SuccessfulRegister);
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> UpdateEducationActivateAsync(int educationId, bool isActive)
        {
            var education = await _unitOfWork.EducationRepository.GetByIdAsync(educationId);
            if (education == null)
                return new ErrorResult(Messages.ObjectIsNull);
            education.IsActive = isActive;
            await _unitOfWork.SaveChangesAsync();

            return new SuccessResult(Messages.Updated);
        }
    }
}
