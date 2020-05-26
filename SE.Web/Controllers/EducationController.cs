using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using LazZiya.ImageResize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SE.Business.CategoryServices;
using SE.Business.EducationServices;
using SE.Business.Helpers;
using SE.Business.ImageServices;
using SE.Core.DTO;
using SE.Web.Model.Education;
using SE.Web.Model.Image;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEducationService _educationService;
        private readonly IImageService _imageService;
        private readonly ICategoryService _categoryService;
        public EducationController(IMapper mapper, IEducationService educationService, IImageService imageService, ICategoryService categoryService)
        {
            _mapper = mapper;
            _educationService = educationService;
            _imageService = imageService;
            _categoryService = categoryService;
        }
        [Authorize]
        [HttpPost("AddEducation")]
        public IActionResult AddEducation([FromBody]EducationInsertModel educationInsertModel)
        {
            try
            {
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) == null || User.FindFirstValue(ClaimTypes.NameIdentifier) == string.Empty)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
                }
                for (int i = 0; i < educationInsertModel.Images.Length; i++)
                {
                    string imageName = $"{UrlHelper.FriendlyUrl(educationInsertModel.GeneralInformation.EducationName)}-{Guid.NewGuid().ToString().Substring(0, 5)}";
                    string base64Data = educationInsertModel.Images[i].Split(',')[1];
                    var bytes = Convert.FromBase64String(base64Data);
                    string filedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    if (bytes.Length > 0)
                    {
                        var bigImage = Image.FromStream(new MemoryStream(bytes));
                        var bigScaleImage = ImageResize.ScaleAndCrop(bigImage, 1000, 600);
                        string bigFile = Path.Combine(filedir, imageName + "_1000x600.jpg");
                        bigScaleImage.SaveAs(bigFile);
                        educationInsertModel.Images[i] = imageName;
                    }
                }
                educationInsertModel.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var educationInsertDto = _mapper.Map<EducationInsertDto>(educationInsertModel);
                int educationId = _educationService.InsertEducation(educationInsertDto);
                return Ok(educationId);
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Errors.Select(d => d.ErrorMessage));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }
        [Authorize]
        [HttpGet("GetAllEducationListByUserId")]
        public IActionResult GetAllEducationListByUserId()
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var educationListModel = _mapper.Map<List<EducationListModel>>(_educationService.GetAllEducationListByUserId(userId));
                educationListModel.ForEach(d => d.DistrictSeoUrl = UrlHelper.FriendlyUrl(d.DistrictName));
                return Ok(educationListModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }
        [Authorize]
        [HttpGet("GetEducationUpdateModelBySeoUrl")]
        public IActionResult GetEducationUpdateModelBySeoUrl(string seoUrl)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var educationUpdateModel = _mapper.Map<EducationUpdateModel>(_educationService.GetEducationUpdateDtoBySeoUrl(seoUrl, userId));
                return Ok(educationUpdateModel);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }

        }
        [HttpGet("GetEducationDetailModelBySeoUrl")]
        public IActionResult GetEducationDetailModelBySeoUrl(string seoUrl)
        {
            try
            {
                var educationDetailModel = _mapper.Map<EducationDetailModel>(_educationService.GetEducationDetailDtoBySeoUrl(seoUrl));
                return Ok(educationDetailModel);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }

        }
        [Authorize]
        [HttpPost("UpdateEducation")]
        public IActionResult UpdateEducation([FromBody]EducationUpdateModel educationUpdateModel)
        {
            try
            {
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) != educationUpdateModel.UserId)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var existingImages = _imageService.GetImagesByEducationId(educationUpdateModel.GeneralInformation.Id);
                if (existingImages != null && educationUpdateModel.Images != null)
                {

                    foreach (var imageUrl in existingImages)
                    {
                        if (!educationUpdateModel.Images.Contains(imageUrl))
                        {
                            string filedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                            string bigFile = Path.Combine(filedir, imageUrl + "_1000x600.jpg");

                            if (System.IO.File.Exists(bigFile))
                            {
                                string smallFile = Path.Combine(filedir, imageUrl + "_300x180.jpg");
                                if (System.IO.File.Exists(smallFile))
                                {
                                    System.IO.File.Delete(smallFile);
                                }
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(bigFile);
                                _imageService.DeleteImageByImageUrl(imageUrl);
                            }
                        }
                    }
                    var newImageList = new List<string>();
                    foreach (var image in educationUpdateModel.Images)
                    {
                        if (image.StartsWith("data:image"))
                        {
                            string imageName = $"{UrlHelper.FriendlyUrl(educationUpdateModel.GeneralInformation.EducationName)}-{Guid.NewGuid().ToString().Substring(0, 5)}.jpg";
                            string base64Data = image.Split(',')[1];
                            var bytes = Convert.FromBase64String(base64Data);
                            string filedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                            if (bytes.Length > 0)
                            {
                                var bigImage = Image.FromStream(new MemoryStream(bytes));
                                var bigScaleImage = ImageResize.ScaleAndCrop(bigImage, 1000, 600);
                                string bigFile = Path.Combine(filedir, imageName + "_1000x600.jpg");
                                bigScaleImage.SaveAs(bigFile);
                                newImageList.Add(imageName);
                            }
                        }
                    }
                    educationUpdateModel.Images = newImageList.ToArray();
                }

                var educationUpdateDto = _mapper.Map<EducationUpdateDto>(educationUpdateModel);
                int educationId =_educationService.UpdateEducation(educationUpdateDto);
                return Ok(educationId);

            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Errors.Select(d => d.ErrorMessage));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }
        [Authorize]
        [HttpDelete("DeleteEducation")]
        public IActionResult DeleteEducation(int educationId)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var educationImages = _mapper.Map<List<ImageModel>>(_educationService.GetAllEducationImageDtoByEducationId(educationId));
                string filedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                foreach (var image in educationImages)
                {
                    string bigFile = Path.Combine(filedir, image.ImageUrl + "_1000x600.jpg");

                    if (System.IO.File.Exists(bigFile))
                    {
                        string smallFile = Path.Combine(filedir, image.ImageUrl + "_300x180.jpg");
                        if (System.IO.File.Exists(smallFile))
                        {
                            System.IO.File.Delete(smallFile);
                            System.GC.Collect();
                            System.GC.WaitForPendingFinalizers();
                        }
                        System.IO.File.Delete(bigFile);
                    }
                }
                _educationService.DeleteEducation(educationId, userId);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }
        [HttpGet("GetAllEducationListByRandomCategoryId")]
        public IActionResult GetAllEducationListByRandomCategoryId()
        {
            try
            {
                var categoryIds = _categoryService.GetAllCategoryList().Select(d => d.Id).ToArray();
                Random random = new Random();
                int randomCategoryId = 1; //categoryIds[random.Next(categoryIds.Length)];
                var educationListModel = _mapper.Map<List<EducationListModel>>(_educationService.GetAllEducationListByCategoryId(randomCategoryId));
                educationListModel.ForEach(d => d.DistrictSeoUrl = UrlHelper.FriendlyUrl(d.DistrictName));
                return Ok(educationListModel);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }
        [HttpGet("GetEducationImagesByEducationId")]
        public IActionResult GetEducationImagesByEducationId(int educationId)
        {
            try
            {
                var educationImages = _mapper.Map<List<ImageModel>>(_educationService.GetAllEducationImageDtoByEducationId(educationId));
                return Ok(educationImages);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }
        [HttpPost("InsertFirstVisibleImage")]
        public IActionResult InsertFirstVisibleImage([FromBody]ImageModel imageModel)
        {
            try
            {
                var imageDto = _mapper.Map<ImageDto>(imageModel);
                _educationService.InsertFirstVisibleImage(imageDto);
                string filedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                string bigFile = Path.Combine(filedir, imageModel.ImageUrl + "_1000x600.jpg");
                string smallFile = Path.Combine(filedir, imageModel.ImageUrl + "_300x180.jpg");
                var bigImage = Image.FromFile(bigFile);
                var bigScaleImage = ImageResize.ScaleAndCrop(bigImage, 300, 180);
                bigScaleImage.SaveAs(smallFile);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }
        [HttpPost("UpdateFirstVisibleImage")]
        public IActionResult UpdateFirstVisibleImage([FromBody]ImageModel imageModel)
        {
            try
            {
                string filedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                string bigFile = Path.Combine(filedir, imageModel.ImageUrl + "_1000x600.jpg");
                string smallFile = Path.Combine(filedir, imageModel.ImageUrl + "_300x180.jpg");
                if (System.IO.File.Exists(smallFile))
                {
                    return Ok();
                }
                var imageDto = _mapper.Map<ImageDto>(imageModel);
                _educationService.UpdateFirstVisibleImage(imageDto);
                var bigImage = Image.FromFile(bigFile);
                var bigScaleImage = ImageResize.ScaleAndCrop(bigImage, 300, 180);
                bigScaleImage.SaveAs(smallFile);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }

        [HttpPost("InsertContactForm")]
        public IActionResult InsertContactForm([FromBody]EducationContactFormInsertModel educationContactFormInsertModel)
        {
            try
            {
                educationContactFormInsertModel.CreateDateTime = DateTime.Now;
                var educationContactFormDto = _mapper.Map<EducationContactFormInsertDto>(educationContactFormInsertModel);
                _educationService.InsertEducationContactForm(educationContactFormDto);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }

        [Authorize]
        [HttpGet("GetEducationContactFormListModelBySeoUrl")]
        public IActionResult GetEducationContactFormListModelBySeoUrl(string seoUrl)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var educationUpdateModel = _mapper.Map<List<EducationContactFormListModel>>(_educationService.GetEducationContactFormListDtoBySeoUrl(seoUrl, userId));
                return Ok(educationUpdateModel);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }

        }
    }
}