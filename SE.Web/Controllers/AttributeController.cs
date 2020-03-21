using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE.Business.AttributeServices;
using SE.Web.Model.Category;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttributeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAttributeService _attributeService;

        public AttributeController(IAttributeService attributeService, IMapper mapper)
        {
            _attributeService = attributeService;
            _mapper = mapper;
        }
        [HttpGet("GetAllAttributeByEducationCategoryId")]
        public IActionResult GetAllAttributeByEducationCategoryId(int categoryId)
        {
            try
            {
                var categoryAttributeListModel = _mapper.Map<List<CategoryAttributeListModel>>(_attributeService.GetAllAttributeByEducationCategoryId(categoryId));
                return Ok(categoryAttributeListModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu.Lütfen işlemi tekrar deneyiniz.");
            }
        }
    }
}