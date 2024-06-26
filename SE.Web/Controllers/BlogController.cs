﻿using System;
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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> InsertBlog([FromBody] BlogInsertModel blogInsertModel)
        {
            var blogInsertDto = _mapper.Map<BlogInsertDto>(blogInsertModel);

            string smallImageName = $"{UrlHelper.FriendlyUrl(blogInsertDto.Title)}-{Path.GetRandomFileName()}";
            string smallImageBase64Data = blogInsertDto.FirstVisibleImageName.Split(',')[1];
            var smallImageBytes = Convert.FromBase64String(smallImageBase64Data);
            string filedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "blog");

            if (smallImageBytes.Length > 0)
            {
                using (var smallScaleImage = SixLabors.ImageSharp.Image.Load(smallImageBytes))
                {
                    var options = new ResizeOptions
                    {
                        Mode = ResizeMode.Crop,
                        Position = AnchorPositionMode.Center,
                        Size = new SixLabors.ImageSharp.Size(300, 180)
                    };
                    smallScaleImage.Mutate(d => d.Resize(options));
                    string smallFile = Path.Combine(filedir, smallImageName + "_300x180.jpg");
                    await smallScaleImage.SaveAsync(smallFile);
                }
                blogInsertDto.FirstVisibleImageName = smallImageName;
            }

            for (int i = 0; i < blogInsertDto.BlogItems.Count; i++)
            {
                if (string.IsNullOrEmpty(blogInsertDto.BlogItems[i].ImageName) && string.IsNullOrEmpty(blogInsertDto.BlogItems[i].Description))
                {
                    blogInsertDto.BlogItems.Remove(blogInsertDto.BlogItems[i]);
                }
            }
            foreach (var blogItem in blogInsertDto.BlogItems)
            {
                if (!string.IsNullOrEmpty(blogItem.ImageName) && blogItem.ImageName.StartsWith("data:image"))
                {
                    string imageName = $"{UrlHelper.FriendlyUrl(blogInsertDto.Title)}_{Path.GetRandomFileName()}";
                    string base64Data = blogItem.ImageName.Split(',')[1];
                    var bytes = Convert.FromBase64String(base64Data);

                    if (bytes.Length > 0)
                    {
                        using (var bigImage = SixLabors.ImageSharp.Image.Load(bytes))
                        {
                            var options = new ResizeOptions
                            {
                                Mode = ResizeMode.Crop,
                                Position = AnchorPositionMode.Center,
                                Size = new SixLabors.ImageSharp.Size(1000, 600)
                            };
                            bigImage.Mutate(d => d.Resize(options));
                            string bigFile = Path.Combine(filedir, imageName + "_1000x600.jpg");
                            await bigImage.SaveAsync(bigFile);
                        }
                        blogItem.ImageName = imageName;
                    }
                }
            }
            var result = await _blogService.InsertBlogAsync(blogInsertDto);
            if (result.Success)
                return Ok(result.Data);

            return BadRequest(result);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost("UpdateBlog")]
        public async Task<IActionResult> UpdateBlog([FromBody] BlogUpdateModel blogUpdateModel)
        {

            var blogUpdateDto = _mapper.Map<BlogUpdateDto>(blogUpdateModel);

            var blogDetail = await _blogService.GetBlogDetailByIdAsync(blogUpdateDto.Id);

            var existingBlogDetailModel = _mapper.Map<BlogDetailModel>(blogDetail.Data);

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
                string firstVisibleImageName = $"{UrlHelper.FriendlyUrl(blogUpdateDto.Title)}_{Path.GetRandomFileName()}.jpg";
                string base64Data = blogUpdateDto.FirstVisibleImageName.Split(',')[1];
                var bytes = Convert.FromBase64String(base64Data);

                if (bytes.Length > 0)
                {
                    using (var firstVisibleImage = SixLabors.ImageSharp.Image.Load(bytes))
                    {
                        var options = new ResizeOptions
                        {
                            Mode = ResizeMode.Crop,
                            Position = AnchorPositionMode.Center,
                            Size = new SixLabors.ImageSharp.Size(300, 180)
                        };
                        firstVisibleImage.Mutate(d => d.Resize(options));
                        string firstVisibleImageFile = Path.Combine(filedir, firstVisibleImageName + "_300x180.jpg");
                        await firstVisibleImage.SaveAsync(firstVisibleImageFile);
                    }
                    blogUpdateDto.FirstVisibleImageName = firstVisibleImageName;
                }
            }


            foreach (var blogItem in blogUpdateDto.BlogItems)
            {
                if (!existingBlogDetailModel.BlogItems.Any(d => d.ImageName == blogItem.ImageName))
                {
                    if (!string.IsNullOrEmpty(blogItem.ImageName) && blogItem.ImageName.StartsWith("data:image"))
                    {
                        string imageName = $"{UrlHelper.FriendlyUrl(blogUpdateDto.Title)}_{Path.GetRandomFileName()}";
                        string base64Data = blogItem.ImageName.Split(',')[1];
                        var bytes = Convert.FromBase64String(base64Data);

                        if (bytes.Length > 0)
                        {
                            using (var bigImage = SixLabors.ImageSharp.Image.Load(bytes))
                            {
                                var options = new ResizeOptions
                                {
                                    Mode = ResizeMode.Crop,
                                    Position = AnchorPositionMode.Center,
                                    Size = new SixLabors.ImageSharp.Size(1000, 600)
                                };
                                bigImage.Mutate(d => d.Resize(options));
                                string bigFile = Path.Combine(filedir, imageName + "_1000x600.jpg");
                                await bigImage.SaveAsync(bigFile);
                            }
                            blogItem.ImageName = imageName;
                        }
                    }
                }
            }

            foreach (var blogItem in existingBlogDetailModel.BlogItems)
            {
                if (!string.IsNullOrEmpty(blogItem.ImageName) && !blogUpdateDto.BlogItems.Any(d => d.ImageName == blogItem.ImageName))
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
                if (string.IsNullOrEmpty(blogUpdateDto.BlogItems[i].ImageName) && string.IsNullOrEmpty(blogUpdateDto.BlogItems[i].Description))
                {
                    blogUpdateDto.BlogItems.Remove(blogUpdateDto.BlogItems[i]);
                }
            }

            var result = await _blogService.UpdateBlogAsync(blogUpdateDto);
            if (result.Success)
                return Ok(result.Data);
            return BadRequest(result);

        }
        [HttpGet("GetAllBlogListByUserId")]
        public async Task<IActionResult> GetAllBlogListByUserId()
        {
            var result = await _blogService.GetAllBlogListByUserIdAsync();
            if (result.Success)
                return Ok(_mapper.Map<List<BlogListModel>>(result.Data));

            return BadRequest(result);
        }

        [HttpGet("GetAllBlogListByUserName")]
        public async Task<IActionResult> GetAllBlogListByUserName(string userName)
        {
            var result = await _blogService.GetAllBlogListByUserNameAsync(userName);
            if (result.Success)
                return Ok(_mapper.Map<List<BlogListModel>>(result.Data));

            return BadRequest(result);
        }
        [HttpGet("GetAllBlogList")]
        public async Task<IActionResult> GetAllBlogList()
        {
            var result = await _blogService.GetAllBlogListAsync();
            if (result.Success)
                return Ok(_mapper.Map<List<BlogListModel>>(result.Data));

            return BadRequest(result);
        }
        [HttpGet("GetAllBlogViewList")]
        public async Task<IActionResult> GetAllBlogViewList(int count)
        {
            var result = await _blogService.GetAllBlogViewListAsync(count);
            if (result.Success)
                return Ok(_mapper.Map<List<BlogListModel>>(result.Data));

            return BadRequest(result);
        }

        [HttpGet("GetBlogDetailBySeoUrl")]
        public async Task<IActionResult> GetBlogDetailBySeoUrl(string seoUrl)
        {
            var result = await _blogService.GetBlogDetailBySeoUrlAsync(seoUrl);
            if (result.Success)
                return Ok(_mapper.Map<BlogDetailModel>(result.Data));

            return NotFound(result);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpGet("GetBlogUpdateBySeoUrl")]
        public async Task<IActionResult> GetBlogUpdateBySeoUrl(string seoUrl)
        {
            var result = await _blogService.GetBlogUpdateBySeoUrlAsync(seoUrl);
            if (result.Success)
                return Ok(_mapper.Map<BlogUpdateModel>(result.Data));

            return BadRequest(result);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpDelete("DeleteBlog")]
        public async Task<IActionResult> DeleteBlog(int blogId)
        {
            var blogDetail = await _blogService.GetBlogDetailByIdAsync(blogId);
            var blogDetailModel = _mapper.Map<BlogDetailModel>(blogDetail.Data);
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
            var result = await _blogService.DeleteBlogAsync(blogId);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("UpdateBlogActivate")]
        public async Task<IActionResult> UpdateBlogActivate([FromBody] BlogActivateModel blogActivateModel)
        {
            var result = await _blogService.UpdateBlogActivateAsync(blogActivateModel.BlogId, blogActivateModel.IsActive);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

    }
}