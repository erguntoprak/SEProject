﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE.Business.AttributeServices;
using SE.Core.DTO;
using SE.Web.Model;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttributeController : ControllerBase
    {
        private readonly IAttributeService _attributeService;
        public AttributeController(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }
        [HttpGet("GetAllAttributeByEducationCategoryId")]
        public IActionResult GetAllAttributeByEducationCategoryId(int categoryId)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel.Data = _attributeService.GetAllAttributeByEducationCategoryId(categoryId);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                responseModel.ErrorMessage.Add("Bilinmeyen bir hata oluştu.Lütfen işlemi tekrar deneyiniz.");
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }
    }
}