using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using LazZiya.ImageResize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE.Business.BlogServices;
using SE.Business.Helpers;
using SE.Core.DTO;
using SE.Web.Model.Blog;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ApiPolicy")]
    public class BlogController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBlogService _blogService;

        public BlogController(IMapper mapper, IBlogService blogService)
        {
            _mapper = mapper;
            _blogService = blogService;
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost("InsertBlog")]
        public IActionResult InsertBlog([FromBody] BlogInsertModel blogInsertModel)
        {
            try
            {
                var blogInsertDto = _mapper.Map<BlogInsertDto>(blogInsertModel);
                blogInsertDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                blogInsertDto.CreateTime = DateTime.Now;
                blogInsertDto.UpdateTime = DateTime.Now;


                string smallImageName = $"{UrlHelper.FriendlyUrl(blogInsertDto.Title)}-{Guid.NewGuid().ToString().Substring(0, 5)}";
                string smallImageBase64Data = blogInsertDto.FirstVisibleImageName.Split(',')[1];
                var smallImageBytes = Convert.FromBase64String(smallImageBase64Data);
                string filedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "blog");

                if (smallImageBytes.Length > 0)
                {
                    var smallImage = Image.FromStream(new MemoryStream(smallImageBytes));
                    var smallScaleImage = ImageResize.ScaleAndCrop(smallImage, 300, 180);
                    string smallFile = Path.Combine(filedir, smallImageName + "_300x180.jpg");
                    smallScaleImage.SaveAs(smallFile);
                    blogInsertDto.FirstVisibleImageName = smallImageName;
                }

                for (int i = 0; i < blogInsertDto.BlogItems.Count; i++)
                {
                    if (blogInsertDto.BlogItems[i].ImageName == "" && blogInsertDto.BlogItems[i].Description == "")
                    {
                        blogInsertDto.BlogItems.Remove(blogInsertDto.BlogItems[i]);
                    }
                }
                foreach (var blogItem in blogInsertDto.BlogItems)
                {
                    if (blogItem.ImageName != "" && blogItem.ImageName.StartsWith("data:image"))
                    {
                        string imageName = $"{UrlHelper.FriendlyUrl(blogInsertDto.Title)}-{Guid.NewGuid().ToString().Substring(0, 5)}";
                        string base64Data = blogItem.ImageName.Split(',')[1];
                        var bytes = Convert.FromBase64String(base64Data);

                        if (bytes.Length > 0)
                        {
                            var bigImage = Image.FromStream(new MemoryStream(bytes));
                            var bigScaleImage = ImageResize.ScaleAndCrop(bigImage, 1000, 600);
                            string bigFile = Path.Combine(filedir, imageName + "_1000x600.jpg");
                            bigScaleImage.SaveAs(bigFile);
                            blogItem.ImageName = imageName;
                        }
                    }
                }
                int blogId = _blogService.InsertBlog(blogInsertDto);
                return Ok(blogId);
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Errors.Select(d => d.ErrorMessage));
            }
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost("UpdateBlog")]
        public IActionResult UpdateBlog([FromBody] BlogUpdateModel blogUpdateModel)
        {
            try
            {
                var blogUpdateDto = _mapper.Map<BlogUpdateDto>(blogUpdateModel);
                blogUpdateDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var existingBlogDetailModel = _mapper.Map<BlogDetailModel>(_blogService.GetBlogDetailById(blogUpdateDto.Id));

                string filedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "blog");

                if (blogUpdateDto.FirstVisibleImageName.StartsWith("data:image"))
                {
                    string existingfirstVisibleImage = Path.Combine(filedir, existingBlogDetailModel.FirstVisibleImageName + "_300x180.jpg");
                    if (System.IO.File.Exists(existingfirstVisibleImage))
                    {
                        System.IO.File.Delete(existingfirstVisibleImage);
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                    }
                    string firstVisibleImageName = $"{UrlHelper.FriendlyUrl(blogUpdateDto.Title)}-{Guid.NewGuid().ToString().Substring(0, 5)}.jpg";
                    string base64Data = blogUpdateDto.FirstVisibleImageName.Split(',')[1];
                    var bytes = Convert.FromBase64String(base64Data);

                    if (bytes.Length > 0)
                    {
                        var firstVisibleImage = Image.FromStream(new MemoryStream(bytes));
                        var firstVisibleImageScale = ImageResize.ScaleAndCrop(firstVisibleImage, 300, 180);
                        string firstVisibleImageFile = Path.Combine(filedir, firstVisibleImageName + "_300x180.jpg");
                        firstVisibleImageScale.SaveAs(firstVisibleImageFile);
                        blogUpdateDto.FirstVisibleImageName = firstVisibleImageName;
                    }
                }


                foreach (var blogItem in blogUpdateDto.BlogItems)
                {
                    if (!existingBlogDetailModel.BlogItems.Any(d => d.ImageName == blogItem.ImageName))
                    {
                        if (blogItem.ImageName != "" && blogItem.ImageName.StartsWith("data:image"))
                        {
                            string imageName = $"{UrlHelper.FriendlyUrl(blogUpdateDto.Title)}-{Guid.NewGuid().ToString().Substring(0, 5)}";
                            string base64Data = blogItem.ImageName.Split(',')[1];
                            var bytes = Convert.FromBase64String(base64Data);

                            if (bytes.Length > 0)
                            {
                                var bigImage = Image.FromStream(new MemoryStream(bytes));
                                var bigScaleImage = ImageResize.ScaleAndCrop(bigImage, 1000, 600);
                                string bigFile = Path.Combine(filedir, imageName + "_1000x600.jpg");
                                bigScaleImage.SaveAs(bigFile);
                                blogItem.ImageName = imageName;
                            }
                        }
                    }
                }

                foreach (var blogItem in existingBlogDetailModel.BlogItems)
                {
                    if (!blogUpdateDto.BlogItems.Any(d => d.ImageName == blogItem.ImageName) && blogItem.ImageName != "")
                    {
                        string existingBlogItemImage = Path.Combine(filedir, blogItem.ImageName + "_1000x600.jpg");
                        if (System.IO.File.Exists(existingBlogItemImage))
                        {
                            System.IO.File.Delete(existingBlogItemImage);
                            System.GC.Collect();
                            System.GC.WaitForPendingFinalizers();
                        }
                    }
                }

                for (int i = 0; i < blogUpdateDto.BlogItems.Count; i++)
                {
                    if (blogUpdateDto.BlogItems[i].ImageName == "" && blogUpdateDto.BlogItems[i].Description == "")
                    {
                        blogUpdateDto.BlogItems.Remove(blogUpdateDto.BlogItems[i]);
                    }
                }
                _blogService.UpdateBlog(blogUpdateDto);
                return Ok();

            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Errors.Select(d => d.ErrorMessage));
            }
        }
        [HttpGet("GetAllBlogListByUserId")]
        public IActionResult GetAllBlogListByUserId()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var blogListModel = _mapper.Map<List<BlogListModel>>(_blogService.GetAllBlogListByUserId(userId));
            return Ok(blogListModel);
        }

        [HttpGet("GetAllBlogListByUserName")]
        public IActionResult GetAllBlogListByUserName(string userName)
        {
            var blogListModel = _mapper.Map<List<BlogListModel>>(_blogService.GetAllBlogListByUserName(userName));
            return Ok(blogListModel);
        }
        [HttpGet("GetAllBlogList")]
        public IActionResult GetAllBlogList()
        {
            var blogListModel = _mapper.Map<List<BlogListModel>>(_blogService.GetAllBlogList());
            return Ok(blogListModel);
        }

        [HttpGet("GetBlogDetailBySeoUrl")]
        public IActionResult GetBlogDetailBySeoUrl(string seoUrl)
        {
            var blogDetailModel = _mapper.Map<BlogDetailModel>(_blogService.GetBlogDetailBySeoUrl(seoUrl));
            return Ok(blogDetailModel);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpGet("GetBlogUpdateBySeoUrl")]
        public IActionResult GetBlogUpdateBySeoUrl(string seoUrl)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var blogUpdateModel = _mapper.Map<BlogUpdateModel>(_blogService.GetBlogUpdateBySeoUrl(seoUrl, userId));
                return Ok(blogUpdateModel);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpDelete("DeleteBlog")]
        public IActionResult DeleteBlog(int blogId)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var blogDetailModel = _mapper.Map<BlogDetailModel>(_blogService.GetBlogDetailById(blogId));
                string filedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "blog");

                var smallImageFile = Path.Combine(filedir, blogDetailModel.FirstVisibleImageName + "_300x180.jpg");

                if (System.IO.File.Exists(smallImageFile))
                {
                    System.IO.File.Delete(smallImageFile);
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                }

                foreach (var blogItem in blogDetailModel.BlogItems)
                {
                    string bigImageFile = Path.Combine(filedir, blogItem.ImageName + "_1000x600.jpg");

                    if (System.IO.File.Exists(bigImageFile))
                    {
                        System.IO.File.Delete(bigImageFile);
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                    }
                }
                _blogService.DeleteBlog(blogId, userId);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
    }
}