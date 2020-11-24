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
            if (result.Success)
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
        public async Task<IActionResult> InsertAttributeCategory([FromBody] AttributeCategoryModel attributeCategoryModel)
        {
            var attributeCategoryDto = _mapper.Map<AttributeCategoryDto>(attributeCategoryModel);
            var result = await _attributeCategoryService.InsertAttributeCategoryAsync(attributeCategoryDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateAttributeCategory")]
        public async Task<IActionResult> UpdateAttributeCategory([FromBody] AttributeCategoryModel attributeCategoryModel)
        {
            var attributeCategoryDto = _mapper.Map<AttributeCategoryDto>(attributeCategoryModel);
            var result = await _attributeCategoryService.UpdateAttributeCategoryAsync(attributeCategoryDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("DeleteAttributeCategory")]
        public async Task<IActionResult> DeleteAttributeCategory([FromBody] int attributeCategoryId)
        {
            var result = await _attributeCategoryService.DeleteAttributeCategoryAsync(attributeCategoryId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("InsertCategoryAttributeCategory")]
        public async Task<IActionResult> InsertCategoryAttributeCategory([FromBody] CategoryAttributeCategoryInsertModel categoryAttributeCategoryInsertModel)
        {
            var categoryAttributeCategoryInsertDto = _mapper.Map<CategoryAttributeCategoryInsertDto>(categoryAttributeCategoryInsertModel);
            var result = await _attributeCategoryService.InsertCategoryAttributeCategoryAsync(categoryAttributeCategoryInsertDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetAttributeCategoryIdsByCategoryId")]
        public async Task<IActionResult> GetAttributeCategoryIdsByCategoryId(int categoryId)
        {
            var result = await _attributeCategoryService.GetAttributeCategoryIdsByCategoryIdAsync(categoryId);
            if (result.Success)
                return Ok(result.Data);
            return BadRequest(result);
        }
    }
}