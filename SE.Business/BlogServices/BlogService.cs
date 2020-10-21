using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SE.Business.Helpers;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.Business.BlogServices
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<BlogInsertDto> _blogInsertDtoValidator;
        private readonly IValidator<BlogUpdateDto> _blogUpdateDtoValidator;


        public BlogService(IUnitOfWork unitOfWork, IValidator<BlogInsertDto> blogInsertDtoValidator, IValidator<BlogUpdateDto> blogUpdateDtoValidator)
        {
            _unitOfWork = unitOfWork;
            _blogInsertDtoValidator = blogInsertDtoValidator;
            _blogUpdateDtoValidator = blogUpdateDtoValidator;
        }
        public void DeleteBlog(int blogId, string userId)
        {
            try
            {
                var blog = _unitOfWork.BlogRepository.Include(d => d.BlogItems).Where(d => d.Id == blogId && d.UserId == userId).FirstOrDefault();
                if (blog != null)
                {
                    _unitOfWork.BlogRepository.Delete(blog);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    throw new ArgumentNullException();
                }

            }
            catch
            {
                throw;
            }
        }

        public List<BlogListDto> GetAllBlogListByUserId(string userId)
        {
            try
            {
                var blogList = _unitOfWork.BlogRepository.Table.Where(d => d.UserId == userId).AsNoTracking().Select(d => new BlogListDto
                {
                    Id = d.Id,
                    CreateTime = d.CreateTime,
                    FirstVisibleImageName = d.FirstVisibleImageName,
                    Title = d.Title,
                    SeoUrl = d.SeoUrl
                }).ToList();
                return blogList;
            }
            catch
            {
                throw;
            }
        }

        public List<BlogListDto> GetAllBlogListByUserName(string userName)
        {
            try
            {
                var blogList = _unitOfWork.BlogRepository.Include(d=>d.User).Where(d => d.User.UserName == userName).AsNoTracking().Select(d => new BlogListDto
                {
                    Id = d.Id,
                    UserName = d.User.UserName,
                    UserSeoUrl = UrlHelper.FriendlyUrl(d.User.UserName),
                    CreateTime = d.CreateTime,
                    FirstVisibleImageName = d.FirstVisibleImageName,
                    Title = d.Title,
                    SeoUrl = d.SeoUrl
                }).ToList();
                return blogList;
            }
            catch
            {
                throw;
            }
        }
        public List<BlogListDto> GetAllBlogList()
        {
            try
            {
                var blogList = _unitOfWork.BlogRepository.Include(d => d.User)
                    .AsNoTracking().Select(d => new BlogListDto
                {
                    Id = d.Id,
                    UserName = d.User.UserName,
                    UserSeoUrl = UrlHelper.FriendlyUrl(d.User.UserName),
                    CreateTime = d.CreateTime,
                    FirstVisibleImageName = d.FirstVisibleImageName,
                    Title = d.Title,
                    SeoUrl = d.SeoUrl
                }).ToList();
                return blogList;
            }
            catch
            {
                throw;
            }
        }
        

        public BlogDetailDto GetBlogDetailById(int Id)
        {
            try
            {
                var blogDetailDto = _unitOfWork.BlogRepository.Include(d => d.BlogItems, u => u.User).Where(d => d.Id == Id).Select(d => new BlogDetailDto
                {
                    Id = d.Id,
                    Author = d.User.UserName,
                    CreateTime = d.CreateTime,
                    Title = d.Title,
                    FirstVisibleImageName = d.FirstVisibleImageName,
                    BlogItems = d.BlogItems.Select(b => new BlogItemDto
                    {
                        Id = b.Id,
                        Description = b.Description,
                        ImageName = b.ImageName
                    }).ToList()
                }).FirstOrDefault();

                if (blogDetailDto != null)
                {
                    return blogDetailDto;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }

        public BlogDetailDto GetBlogDetailBySeoUrl(string seoUrl)
        {
            try
            {
                var blogDetailDto = _unitOfWork.BlogRepository.Include(d => d.BlogItems, u => u.User).Where(d => d.SeoUrl == seoUrl).Select(d => new BlogDetailDto
                {
                    Id = d.Id,
                    Author = d.User.UserName,
                    CreateTime = d.CreateTime,
                    Title = d.Title,
                    FirstVisibleImageName = d.FirstVisibleImageName,
                    BlogItems = d.BlogItems.Select(b => new BlogItemDto
                    {
                        Id = b.Id,
                        Description = b.Description,
                        ImageName = b.ImageName
                    }).ToList()
                }).FirstOrDefault();
                return blogDetailDto;
            }
            catch
            {
                throw;
            }
        }

        public BlogUpdateDto GetBlogUpdateBySeoUrl(string seoUrl, string userId)
        {
            try
            {
                var blogUpdateDto = _unitOfWork.BlogRepository.Include(d => d.BlogItems).Where(d => d.SeoUrl == seoUrl && d.UserId == userId).Select(d => new BlogUpdateDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    UserId = d.UserId,
                    FirstVisibleImageName = d.FirstVisibleImageName,
                    BlogItems = d.BlogItems.Select(b => new BlogItemDto
                    {
                        Id = b.Id,
                        Description = b.Description,
                        ImageName = b.ImageName
                    }).ToList()
                }).FirstOrDefault();

                if (blogUpdateDto == null)
                {
                    throw new ArgumentNullException();
                }
                return blogUpdateDto;
            }
            catch
            {
                throw;
            }
        }

        public int InsertBlog(BlogInsertDto blogInsertDto)
        {
            try
            {
                var blogInsertDtoValidate = _blogInsertDtoValidator.Validate(blogInsertDto, ruleSet: "all");
                if (!blogInsertDtoValidate.IsValid)
                {
                    throw new ValidationException(blogInsertDtoValidate.Errors);
                }
                var seoUrl = UrlHelper.FriendlyUrl(blogInsertDto.Title);
                var savedSeoUrl = _unitOfWork.BlogRepository.Table.Where(d => d.SeoUrl == seoUrl).Select(d => d.SeoUrl).AsNoTracking().FirstOrDefault();

                if (savedSeoUrl != null)
                {
                    seoUrl += Guid.NewGuid().ToString().Substring(0, 5);
                }

                var blog = new Blog()
                {
                    UserId = blogInsertDto.UserId,
                    Title = blogInsertDto.Title,
                    CreateTime = blogInsertDto.CreateTime,
                    UpdateTime = blogInsertDto.UpdateTime,
                    FirstVisibleImageName = blogInsertDto.FirstVisibleImageName,
                    SeoUrl = seoUrl
                };
                foreach (var blogItem in blogInsertDto.BlogItems)
                {
                    blog.BlogItems.Add(new BlogItem
                    {
                        ImageName = blogItem.ImageName,
                        Description = blogItem.Description
                    });
                }
                _unitOfWork.BlogRepository.Insert(blog);
                _unitOfWork.SaveChanges();
                return blog.Id;
            }
            catch
            {
                throw;
            }
        }

        public int UpdateBlog(BlogUpdateDto blogUpdateDto)
        {
            try
            {
                var blogInsertDtoValidate = _blogUpdateDtoValidator.Validate(blogUpdateDto, ruleSet: "all");
                if (!blogInsertDtoValidate.IsValid)
                {
                    throw new ValidationException(blogInsertDtoValidate.Errors);
                }
                var blog = _unitOfWork.BlogRepository.Include(d => d.BlogItems).Where(d => d.Id == blogUpdateDto.Id && d.UserId == blogUpdateDto.UserId).FirstOrDefault();

                if (blog != null)
                {
                    blog.Title = blogUpdateDto.Title;
                    var seoUrl = UrlHelper.FriendlyUrl(blogUpdateDto.Title);

                    if (seoUrl != blog.SeoUrl)
                    {
                        string savedSeoUrl = _unitOfWork.BlogRepository.Table.Where(d => d.SeoUrl == seoUrl).Select(d => d.SeoUrl).FirstOrDefault();

                        if (savedSeoUrl != null)
                        {
                            blog.SeoUrl = seoUrl + Guid.NewGuid().ToString().Substring(0, 5);
                        }
                        else
                        {
                            blog.SeoUrl = seoUrl;
                        }
                    }

                    blog.UpdateTime = DateTime.Now;
                    blog.FirstVisibleImageName = blogUpdateDto.FirstVisibleImageName;
                    blog.BlogItems.Clear();

                    foreach (var blogItem in blogUpdateDto.BlogItems)
                    {
                        blog.BlogItems.Add(new BlogItem
                        {
                            ImageName = blogItem.ImageName,
                            Description = blogItem.Description
                        });
                    }

                    _unitOfWork.BlogRepository.Update(blog);
                    _unitOfWork.SaveChanges();
                    return blog.Id;
                }
                else
                {
                    throw new ArgumentNullException();
                }

            }
            catch
            {
                throw;
            }
        }
    }
}
