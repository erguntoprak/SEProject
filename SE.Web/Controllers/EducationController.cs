using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SE.Business.EducationServices;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Web.Infrastructure.Helpers;
using SE.Web.Model;

namespace SE.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEducationService _educationService; 
        public EducationController(IMapper mapper, IEducationService educationService)
        {
            _mapper = mapper;
            _educationService = educationService;
        }
        [HttpPost("AddEducation")]
        public IActionResult AddEducationAsync([FromBody]EducationInsertModel educationInsertModel)
        {
            try
            {
                educationInsertModel.GeneralInformation.UserId = User.FindFirstValue(ClaimTypes.Name);
                educationInsertModel.GeneralInformation.SeoUrl = UrlHelper.FriendlyUrl(educationInsertModel.GeneralInformation.EducationName);
                if (educationInsertModel !=null)
                {
                    var educationInsertDto = _mapper.Map<EducationInsertDto>(educationInsertModel);
                    _educationService.InsertEducation(educationInsertDto);
                    return Ok();
                }
                return StatusCode(StatusCodes.Status400BadRequest);

            }
            catch (Exception ex) 
            {

                throw ex;
            }
        }
        [HttpGet("GetAllEducationList")]
        public IActionResult GetAllEducationList()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.Name);
                responseModel.Data = _educationService.GetAllEducationListByUserId(userId);
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