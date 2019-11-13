using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE.Business.CategoryServices;
using SE.Core.DTO;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService; 
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("GetAllCategoryList")]
        public List<CategoryDto> GetAllCategoryList()
        {
            try
            {
                return _categoryService.GetAllCategoryList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}