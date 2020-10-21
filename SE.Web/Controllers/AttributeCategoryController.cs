using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE.Business.AttributeCategoryServices;
using SE.Core.DTO;
using SE.Web.Model.Attribute;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ApiPolicy")]
    public class AttributeCategoryController : ControllerBase
    {
        private readonly IAttributeCategoryService _attributeCategoryService;
        private readonly IMapper _mapper;
        public AttributeCategoryController(IAttributeCategoryService attributeCategoryService, IMapper mapper)
        {
            _attributeCategoryService = attributeCategoryService;
            _mapper = mapper;
        }
        [HttpGet("GetAllAttributeCategoryList")]
        public IActionResult GetAllAttributeCategoryList()
        {
            try
            {
                var attributeCategoryList = _mapper.Map<List<AttributeCategoryModel>>(_attributeCategoryService.GetAllAttributeCategoryList());
                return Ok(attributeCategoryList);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }

        [HttpGet("GetAttributeCategoryById")]
        public IActionResult GetAttributeCategoryById([FromQuery]int attributeCategoryId)
        {
            try
            {
                var attributeCategoryModel = _mapper.Map<AttributeCategoryModel>(_attributeCategoryService.GetAttributeCategoryById(attributeCategoryId));
                return Ok(attributeCategoryModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("InsertAttributeCategory")]
        public IActionResult InsertAttributeCategory([FromBody]AttributeCategoryModel attributeCategoryModel)
        {
            try
            {
                var attributeCategoryDto = _mapper.Map<AttributeCategoryDto>(attributeCategoryModel);
                _attributeCategoryService.InsertAttributeCategory(attributeCategoryDto);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }
        
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateAttributeCategory")]
        public IActionResult UpdateAttributeCategory([FromBody]AttributeCategoryModel attributeCategoryModel)
        {
            try
            {
                var attributeCategoryDto = _mapper.Map<AttributeCategoryDto>(attributeCategoryModel);
                _attributeCategoryService.UpdateAttributeCategory(attributeCategoryDto);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("DeleteAttributeCategory")]
        public IActionResult DeleteAttributeCategory([FromBody]int attributeCategoryId)
        {
            try
            {
                _attributeCategoryService.DeleteAttributeCategory(attributeCategoryId);
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bu kaydı silemezsin. Silmek için bağlı olduğu diğer kayıtların silinmesi gerekiyor.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("InsertCategoryAttributeCategory")]
        public IActionResult InsertCategoryAttributeCategory([FromBody]CategoryAttributeCategoryInsertModel categoryAttributeCategoryInsertModel)
        {
            try
            {
                var categoryAttributeCategoryInsertDto = _mapper.Map<CategoryAttributeCategoryInsertDto>(categoryAttributeCategoryInsertModel);
                _attributeCategoryService.InsertCategoryAttributeCategory(categoryAttributeCategoryInsertDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }

        [HttpGet("GetAttributeCategoryIdsByCategoryId")]
        public IActionResult GetAttributeCategoryIdsByCategoryId(int categoryId)
        {
            try
            {
                var attributeCategoryIds = _attributeCategoryService.GetAttributeCategoryIdsByCategoryId(categoryId);
                return Ok(attributeCategoryIds);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }
    }
}