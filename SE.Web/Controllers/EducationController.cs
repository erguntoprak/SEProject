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
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE.Business.CategoryServices;
using SE.Business.EducationServices;
using SE.Business.ImageServices;
using SE.Core.DTO;
using SE.Web.Model.Education;
using SE.Web.Model.Image;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

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
        [Authorize(Policy = "UserPolicy")]
        [HttpPost("AddEducation")]
        public async Task<IActionResult> AddEducation([FromBody] EducationInsertModel educationInsertModel)
        {

            educationInsertModel.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var educationInsertDto = _mapper.Map<EducationInsertDto>(educationInsertModel);
            var result = await _educationService.InsertEducationAsync(educationInsertDto);
            if (result.Success)
                return Ok(result.Data);

            return BadRequest(result);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost("UploadEducationImage/{educationId}")]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [RequestSizeLimit(409715200)]
        public async Task<IActionResult> UploadEducationImage([FromRoute] int educationId)
        {
            EducationUploadImageDto educationUploadImageDto = new EducationUploadImageDto();
            educationUploadImageDto.EducationId = educationId;
            educationUploadImageDto.UploadImages = Request.Form.Files;
            educationUploadImageDto.Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            var result = await _educationService.UploadEducationImageAsync(educationUploadImageDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [Authorize(Policy = "UserPolicy")]
        [HttpGet("GetAllEducationListByUserId")]
        public async Task<IActionResult> GetAllEducationListByUserId()
        {
            var result = await _educationService.GetAllEducationListByUserIdAsync();
            if (result.Success)
                return Ok(_mapper.Map<List<EducationListModel>>(result.Data));

            return BadRequest(result);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpGet("GetEducationUpdateModelBySeoUrl")]
        public async Task<IActionResult> GetEducationUpdateModelBySeoUrl(string seoUrl)
        {
            var result = await _educationService.GetEducationUpdateDtoBySeoUrlAsync(seoUrl);
            if (result.Success)
                return Ok(_mapper.Map<EducationUpdateModel>(result.Data));

            return BadRequest(result);
        }

        [HttpGet("GetEducationDetailModelBySeoUrl")]
        public async Task<IActionResult> GetEducationDetailModelBySeoUrl(string seoUrl)
        {
            var result = await _educationService.GetEducationDetailDtoBySeoUrlAsync(seoUrl);
            if (result.Success)
                return Ok(_mapper.Map<EducationDetailModel>(result.Data));
            return NotFound(result);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost("UpdateEducation")]
        public async Task<IActionResult> UpdateEducation([FromBody] EducationUpdateModel educationUpdateModel)
        {
            var existingImages = await _imageService.GetImagesByEducationIdAsync(educationUpdateModel.GeneralInformation.Id);

            if (existingImages != null && educationUpdateModel.Images != null)
            {
                foreach (var imageUrl in existingImages.Data)
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
                            await _imageService.DeleteImageByImageUrlAsync(imageUrl);
                        }
                    }
                }
            }

            var educationUpdateDto = _mapper.Map<EducationUpdateDto>(educationUpdateModel);
            var result = await _educationService.UpdateEducationAsync(educationUpdateDto);
            if (result.Success)
                return Ok(result.Data);

            return BadRequest(result);
        }


        [Authorize(Policy = "UserPolicy")]
        [HttpDelete("DeleteEducation")]
        public async Task<IActionResult> DeleteEducation(int educationId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var educationImages = await _educationService.GetAllEducationImageDtoByEducationIdAsync(educationId);

            var educationImageModels = _mapper.Map<List<ImageModel>>(educationImages.Data);
            string filedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            foreach (var image in educationImageModels)
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

            var result = await _educationService.DeleteEducationAsync(educationId);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("GetAllEducationListByRandomCategoryId")]
        public async Task<IActionResult> GetAllEducationListByRandomCategoryId()
        {
            var categoryList = await _categoryService.GetAllCategoryListAsync();
            var categoryIds = categoryList.Data.Select(d => d.Id).ToArray();
            Random random = new Random();
            int randomCategoryId = categoryIds[random.Next(categoryIds.Length)];
            var result = await _educationService.GetAllEducationListByCategoryIdAndDistrictIdAsync(randomCategoryId, 0);
            if (result.Success)
                return Ok(_mapper.Map<List<EducationListModel>>(result.Data));

            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("GetAllEducationList")]
        public async Task<IActionResult> GetAllEducationList()
        {
            var result = await _educationService.GetAllEducationListAsync();
            if (result.Success)
                return Ok(_mapper.Map<List<EducationListModel>>(result.Data));
            return BadRequest(result);
        }


        [HttpGet("GetAllEducationListByCategoryIdAndDistrictId")]
        public async Task<IActionResult> GetAllEducationListByCategoryIdAndDistrictId(int categoryId, int districtId)
        {

            var result = await _educationService.GetAllEducationListByCategoryIdAndDistrictIdAsync(categoryId, districtId);
            if (result.Success)
                return Ok(_mapper.Map<List<EducationListModel>>(result.Data));

            return BadRequest(result);
        }

        [HttpGet("GetAllEducationListByFilter")]
        public async Task<IActionResult> GetAllEducationListByFilter([FromQuery] FilterModel filterModel)
        {

            var filterDto = _mapper.Map<FilterDto>(filterModel);
            var result = await _educationService.GetAllEducationListByFilterAsync(filterDto);
            if (result.Success)
                return Ok(_mapper.Map<List<EducationFilterListModel>>(result.Data));

            return BadRequest(result);
        }


        [HttpGet("GetEducationImagesByEducationId")]
        public async Task<IActionResult> GetEducationImagesByEducationId(int educationId)
        {
            var result = await _educationService.GetAllEducationImageDtoByEducationIdAsync(educationId);
            if (result.Success)
                return Ok(_mapper.Map<List<ImageModel>>(result.Data));

            return BadRequest(result);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost("InsertFirstVisibleImage")]
        public async Task<IActionResult> InsertFirstVisibleImage([FromBody] ImageModel imageModel)
        {

            var imageDto = _mapper.Map<ImageDto>(imageModel);
            var result = await _educationService.InsertFirstVisibleImageAsync(imageDto);
            string filedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            string bigFile = Path.Combine(filedir, imageModel.ImageUrl + "_1000x600.jpg");
            string smallFile = Path.Combine(filedir, imageModel.ImageUrl + "_300x180.jpg");

            using (var bigImage = SixLabors.ImageSharp.Image.Load(bigFile))
            {
                var options = new ResizeOptions
                {
                    Mode = ResizeMode.Crop,
                    Position = AnchorPositionMode.Center,
                    Size = new SixLabors.ImageSharp.Size(300, 180)
                };
                bigImage.Mutate(d => d.Resize(options));
                await bigImage.SaveAsync(smallFile);
            }

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost("UpdateFirstVisibleImage")]
        public async Task<IActionResult> UpdateFirstVisibleImage([FromBody] ImageModel imageModel)
        {

            string filedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            string bigFile = Path.Combine(filedir, imageModel.ImageUrl + "_1000x600.jpg");
            string smallFile = Path.Combine(filedir, imageModel.ImageUrl + "_300x180.jpg");

            var educationImages = await _educationService.GetAllEducationImageDtoByEducationIdAsync(imageModel.EducationId);
            var educationImageModels = _mapper.Map<List<ImageModel>>(educationImages.Data);

            foreach (var image in educationImageModels)
            {
                string existingSmallFile = Path.Combine(filedir, image.ImageUrl + "_300x180.jpg");
                if (System.IO.File.Exists(existingSmallFile))
                {
                    System.IO.File.Delete(existingSmallFile);
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                }
            }

            var imageDto = _mapper.Map<ImageDto>(imageModel);
            var result = await _educationService.UpdateFirstVisibleImageAsync(imageDto);
            using (var bigImage = SixLabors.ImageSharp.Image.Load(bigFile))
            {
                var options = new ResizeOptions
                {
                    Mode = ResizeMode.Crop,
                    Position = AnchorPositionMode.Center,
                    Size = new SixLabors.ImageSharp.Size(300, 180)
                };
                bigImage.Mutate(d => d.Resize(options));
                await bigImage.SaveAsync(smallFile);
            }

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("InsertContactForm")]
        public async Task<IActionResult> InsertContactForm([FromBody] EducationContactFormInsertModel educationContactFormInsertModel)
        {
            educationContactFormInsertModel.CreateDateTime = DateTime.Now;
            var educationContactFormDto = _mapper.Map<EducationContactFormInsertDto>(educationContactFormInsertModel);
            var result = await _educationService.InsertEducationContactFormAsync(educationContactFormDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpGet("GetEducationContactFormListModelBySeoUrl")]
        public async Task<IActionResult> GetEducationContactFormListModelBySeoUrl(string seoUrl)
        {
            var result = await _educationService.GetEducationContactFormListDtoBySeoUrlAsync(seoUrl);
            if (result.Success)
                return Ok(_mapper.Map<List<EducationContactFormListModel>>(result.Data));

            return BadRequest(result);
        }

        [HttpGet("GetAllSearchEducationList")]
        public async Task<IActionResult> GetAllSearchEducationList()
        {
            var result = await _educationService.GetAllSearchEducationListAsync();
            if (result.Success)
                return Ok(_mapper.Map<List<SearchEducationModel>>(result.Data));

            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateEducationActivate")]
        public async Task<IActionResult> UpdateEducationActivate([FromBody] EducationActivateModel educationActivateModel)
        {
            var result = await _educationService.UpdateEducationActivateAsync(educationActivateModel.EducationId, educationActivateModel.IsActive);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

    }
}