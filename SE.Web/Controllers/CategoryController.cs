using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE.Business.CategoryServices;
using SE.Core.DTO;
using SE.Web.Model;

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
        public IActionResult GetAllCategoryList()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel.Data = _categoryService.GetAllCategoryList();
                return Ok(responseModel);
            }
            catch (Exception)
            {
                responseModel.ErrorMessage.Add("Bilinmeyen bir hata oluştu.Lütfen işlemi tekrar deneyiniz.");
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }
    }
}