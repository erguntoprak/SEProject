using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE.Business.CategoryServices;
using SE.Web.Model.Category;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                var categoryModel = _mapper.Map<List<CategoryModel>>(_categoryService.GetAllCategoryList());
                return Ok(categoryModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }
    }
}