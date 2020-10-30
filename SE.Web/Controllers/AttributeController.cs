using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE.Business.AttributeServices;
using SE.Core.DTO;
using SE.Web.Model.Attribute;
using SE.Web.Model.Category;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ApiPolicy")]
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
            var categoryAttributeListModel = _mapper.Map<List<CategoryAttributeListModel>>(_attributeService.GetAllAttributeByEducationCategoryId(categoryId));
            return Ok(categoryAttributeListModel);
        }

        [HttpGet("GetAllAttributeList")]
        public IActionResult GetAllAttributeList()
        {
            var attributeListModel = _mapper.Map<List<AttributeListModel>>(_attributeService.GetAllAttributeList());
            return Ok(attributeListModel);
        }

        [HttpGet("GetAttributeById")]
        public IActionResult GetAttributeById([FromQuery] int attributeId)
        {
            var attributeModel = _mapper.Map<AttributeModel>(_attributeService.GetAttributeById(attributeId));
            return Ok(attributeModel);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("InsertAttribute")]
        public IActionResult InsertAttribute([FromBody] AttributeModel attributeModel)
        {
            var attributeDto = _mapper.Map<AttributeDto>(attributeModel);
            _attributeService.InsertAttribute(attributeDto);
            return Ok();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateAttribute")]
        public IActionResult UpdateAttribute([FromBody] AttributeModel attributeModel)
        {
            var attributeDto = _mapper.Map<AttributeDto>(attributeModel);
            _attributeService.UpdateAttribute(attributeDto);
            return Ok();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("DeleteAttribute")]
        public IActionResult DeleteAttribute([FromBody] int attributeId)
        {
            try
            {
                _attributeService.DeleteAttribute(attributeId);
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bu kaydı silemezsin. Silmek için bağlı olduğu diğer kayıtların silinmesi gerekiyor.");
            }
        }

    }
}