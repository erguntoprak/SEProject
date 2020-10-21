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
            try
            {
                var categoryList = _mapper.Map<List<CategoryModel>>(_categoryService.GetAllCategoryList());
                return Ok(categoryList);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }

        [HttpGet("GetCategoryById")]
        public IActionResult GetCategoryById([FromQuery]int categoryId)
        {
            try
            {
                var categoryModel = _mapper.Map<CategoryModel>(_categoryService.GetCategoryById(categoryId));
                return Ok(categoryModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("InsertCategory")]
        public IActionResult InsertCategory([FromBody]CategoryModel categoryModel)
        {
            try
            {
                var categoryDto = _mapper.Map<CategoryDto>(categoryModel);
                _categoryService.InsertCategory(categoryDto);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateCategory")]
        public IActionResult UpdateCategory([FromBody]CategoryModel categoryModel)
        {
            try
            {
                var categoryDto = _mapper.Map<CategoryDto>(categoryModel);
                _categoryService.UpdateCategory(categoryDto);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("DeleteCategory")]
        public IActionResult DeleteCategory([FromBody]int categoryId)
        {
            try
            {
                _categoryService.DeleteCategory(categoryId);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }
    }
}