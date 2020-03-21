using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SE.Business.EducationServices;
using SE.Business.Helpers;
using SE.Business.ImageServices;
using SE.Core.DTO;
using SE.Web.Model.Education;

namespace SE.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEducationService _educationService;
        private readonly IImageService _imageService;
        public EducationController(IMapper mapper, IEducationService educationService, IImageService imageService)
        {
            _mapper = mapper;
            _educationService = educationService;
            _imageService = imageService;
        }
        [HttpPost("AddEducation")]
        public IActionResult AddEducationAsync([FromBody]EducationInsertModel educationInsertModel)
        {
            try
            {
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) == null || User.FindFirstValue(ClaimTypes.NameIdentifier) == string.Empty)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
                }
                for (int i = 0; i < educationInsertModel.Images.Length; i++)
                {
                    string imageName = $"{UrlHelper.FriendlyUrl(educationInsertModel.GeneralInformation.EducationName)}-{Guid.NewGuid().ToString().Substring(0, 5)}.jpg";
                    string base64Data = educationInsertModel.Images[i].Split(',')[1];
                    var bytes = Convert.FromBase64String(base64Data);
                    string filedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    string file = Path.Combine(filedir, imageName);

                    if (bytes.Length > 0)
                    {
                        using (var stream = new FileStream(file, FileMode.Create))
                        {
                            stream.Write(bytes, 0, bytes.Length);
                            stream.Flush();
                        }
                    }
                    educationInsertModel.Images[i] = imageName;
                }
                educationInsertModel.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var educationInsertDto = _mapper.Map<EducationInsertDto>(educationInsertModel);
                _educationService.InsertEducation(educationInsertDto);
                return Ok();
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
        [HttpGet("GetAllEducationList")]
        public IActionResult GetAllEducationList()
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var educationListModel = _mapper.Map<List<EducationListModel>>(_educationService.GetAllEducationListByUserId(userId));
                return Ok(educationListModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }

        [HttpGet("GetEducationUpdateModelEditDtoBySeoUrl")]
        public IActionResult GetEducationUpdateModelEditDtoBySeoUrl(string seoUrl)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var educationUpdateModel = _mapper.Map<EducationUpdateModel>(_educationService.GetEducationUpdateDtoEditDtoBySeoUrl(seoUrl,userId));
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
                            string file = Path.Combine(filedir, imageUrl);

                            if (System.IO.File.Exists(file))
                            {
                                System.IO.File.Delete(file);
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
                            string file = Path.Combine(filedir, imageName);

                            if (bytes.Length > 0)
                            {
                                using (var stream = new FileStream(file, FileMode.Create))
                                {
                                    stream.Write(bytes, 0, bytes.Length);
                                    stream.Flush();
                                }
                            }
                            newImageList.Add(imageName);
                        }
                    }
                    educationUpdateModel.Images = newImageList.ToArray();
                }

                var educationUpdateDto = _mapper.Map<EducationUpdateDto>(educationUpdateModel);
                _educationService.UpdateEducation(educationUpdateDto);
                return Ok();

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
        [HttpDelete("DeleteEducation")]
        public IActionResult DeleteEducation(int educationId)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
    }
}