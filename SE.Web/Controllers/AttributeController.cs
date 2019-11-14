using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE.Business.AttributeServices;
using SE.Core.DTO;
using SE.Web.Model;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeController : ControllerBase
    {
        private readonly IAttributeService _attributeService;
        public AttributeController(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }
        [HttpGet("GetAllAttributeList")]
        public IActionResult GetAllAttributeList()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel.Data = _attributeService.GetAllEducationAttributeList();
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