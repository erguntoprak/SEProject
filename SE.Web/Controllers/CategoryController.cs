using System;
using System.Collections.Generic;
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
        public IActionResult GetAllCategoryList()
        {
            var categoryList = _mapper.Map<List<CategoryModel>>(_categoryService.GetAllCategoryList());
            return Ok(categoryList);
        }

        [HttpGet("GetCategoryById")]
        public IActionResult GetCategoryById([FromQuery] int categoryId)
        {
            var categoryModel = _mapper.Map<CategoryModel>(_categoryService.GetCategoryById(categoryId));
            return Ok(categoryModel);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("InsertCategory")]
        public IActionResult InsertCategory([FromBody] CategoryModel categoryModel)
        {
            var categoryDto = _mapper.Map<CategoryDto>(categoryModel);
            _categoryService.InsertCategory(categoryDto);
            return Ok();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateCategory")]
        public IActionResult UpdateCategory([FromBody] CategoryModel categoryModel)
        {
            var categoryDto = _mapper.Map<CategoryDto>(categoryModel);
            _categoryService.UpdateCategory(categoryDto);
            return Ok();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("DeleteCategory")]
        public IActionResult DeleteCategory([FromBody] int categoryId)
        {
            _categoryService.DeleteCategory(categoryId);
            return Ok();
        }
    }
}