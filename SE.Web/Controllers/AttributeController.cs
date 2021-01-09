using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GetAllAttributeByEducationCategoryId(int categoryId)
        {
            var result = await _attributeService.GetAllAttributeByEducationCategoryIdAsync(categoryId);
            if (result.Success)
                return Ok(_mapper.Map<List<CategoryAttributeListModel>>(result.Data));

            return BadRequest(result);
        }

        [HttpGet("GetAllAttributeList")]
        public async Task<IActionResult> GetAllAttributeList()
        {
            var result = await _attributeService.GetAllAttributeListAsync();
            if (result.Success)
                return Ok(_mapper.Map<List<AttributeListModel>>(result.Data));

            return BadRequest(result);
        }

        [HttpGet("GetAttributeById")]
        public async Task<IActionResult> GetAttributeById([FromQuery] int attributeId)
        {
            var result = await _attributeService.GetAttributeByIdAsync(attributeId);
            if (result.Success)
                return Ok(_mapper.Map<AttributeModel>(result.Data));

            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("InsertAttribute")]
        public async Task<IActionResult> InsertAttribute([FromBody] AttributeModel attributeModel)
        {
            var attributeDto = _mapper.Map<AttributeDto>(attributeModel);
            var result = await _attributeService.InsertAttributeAsync(attributeDto);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateAttribute")]
        public async Task<IActionResult> UpdateAttribute([FromBody] AttributeModel attributeModel)
        {
            var attributeDto = _mapper.Map<AttributeDto>(attributeModel);
            var result = await _attributeService.UpdateAttributeAsync(attributeDto);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("DeleteAttribute")]
        public async Task<IActionResult> DeleteAttribute([FromBody] int attributeId)
        {

            var result = await _attributeService.DeleteAttributeAsync(attributeId);
            if (result.Success)
                return Ok(result);
            return BadRequest();
        }

    }
}