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
        public async Task<IActionResult> GetAllAttributeCategoryList()
        {
            var result = await _attributeCategoryService.GetAllAttributeCategoryListAsync();
            if(result.Success)
                return Ok(_mapper.Map<List<AttributeCategoryModel>>(result.Data));

            return BadRequest(result);
            
        }

        [HttpGet("GetAttributeCategoryById")]
        public async Task<IActionResult> GetAttributeCategoryById([FromQuery] int attributeCategoryId)
        {
            var result = await _attributeCategoryService.GetAttributeCategoryByIdAsync(attributeCategoryId);
            if (result.Success)
                return Ok(_mapper.Map<AttributeCategoryModel>(result.Data));

            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("InsertAttributeCategory")]
        public IActionResult InsertAttributeCategory([FromBody] AttributeCategoryModel attributeCategoryModel)
        {
            var attributeCategoryDto = _mapper.Map<AttributeCategoryDto>(attributeCategoryModel);
            _attributeCategoryService.InsertAttributeCategory(attributeCategoryDto);
            return Ok();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateAttributeCategory")]
        public IActionResult UpdateAttributeCategory([FromBody] AttributeCategoryModel attributeCategoryModel)
        {
            var attributeCategoryDto = _mapper.Map<AttributeCategoryDto>(attributeCategoryModel);
            _attributeCategoryService.UpdateAttributeCategory(attributeCategoryDto);
            return Ok();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("DeleteAttributeCategory")]
        public IActionResult DeleteAttributeCategory([FromBody] int attributeCategoryId)
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
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("InsertCategoryAttributeCategory")]
        public IActionResult InsertCategoryAttributeCategory([FromBody] CategoryAttributeCategoryInsertModel categoryAttributeCategoryInsertModel)
        {
            var categoryAttributeCategoryInsertDto = _mapper.Map<CategoryAttributeCategoryInsertDto>(categoryAttributeCategoryInsertModel);
            _attributeCategoryService.InsertCategoryAttributeCategory(categoryAttributeCategoryInsertDto);
            return Ok();
        }

        [HttpGet("GetAttributeCategoryIdsByCategoryId")]
        public IActionResult GetAttributeCategoryIdsByCategoryId(int categoryId)
        {
            var attributeCategoryIds = _attributeCategoryService.GetAttributeCategoryIdsByCategoryId(categoryId);
            return Ok(attributeCategoryIds);
        }
    }
}