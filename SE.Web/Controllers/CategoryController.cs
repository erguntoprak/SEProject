using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE.Business.CategoryServices;
using SE.Core.DTO;
using SE.Web.Model.Category;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ApiPolicy")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        [HttpGet("GetAllCategoryList")]
        public async Task<IActionResult> GetAllCategoryList()
        {
            var result = await _categoryService.GetAllCategoryListAsync();
            if (result.Success)
                return Ok(_mapper.Map<List<CategoryModel>>(result.Data));

            return BadRequest(result);
        }

        [HttpGet("GetCategoryById")]
        public async Task<IActionResult> GetCategoryById([FromQuery] int categoryId)
        {
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);
            if (result.Success)
                return Ok(_mapper.Map<CategoryModel>(result.Data));

            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("InsertCategory")]
        public async Task<IActionResult> InsertCategory([FromBody] CategoryModel categoryModel)
        {
            var categoryDto = _mapper.Map<CategoryDto>(categoryModel);
            var result = await _categoryService.InsertCategoryAsync(categoryDto);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryModel categoryModel)
        {
            var categoryDto = _mapper.Map<CategoryDto>(categoryModel);
            var result = await _categoryService.UpdateCategoryAsync(categoryDto);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory([FromBody] int categoryId)
        {
            var result = await _categoryService.DeleteCategoryAsync(categoryId);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}