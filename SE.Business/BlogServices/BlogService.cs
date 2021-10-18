using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SE.Business.Constants;
using SE.Business.Helpers;
using SE.Business.Infrastructure.FluentValidation.Validations;
using SE.Core.Aspects.Autofac.Validation;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Core.Utilities.Results;
using SE.Core.Utilities.Security.Http;
using SE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Business.BlogServices
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestContext _requestContext;

        public BlogService(IUnitOfWork unitOfWork, IRequestContext requestContext)
        {
            _unitOfWork = unitOfWork;
            _requestContext = requestContext;
        }
        public async Task<IResult> DeleteBlogAsync(int blogId)
        {
            var blog = await _unitOfWork.BlogRepository.Include(d => d.BlogItems).Where(d => d.Id == blogId && (_requestContext.Roles.Contains(GeneralConstants.Admin) ? true : d.UserId == _requestContext.UserId)).FirstOrDefaultAsync();

            if (blog == null)
                return new ErrorResult(Messages.ObjectIsNull);

            _unitOfWork.BlogRepository.Delete(blog);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult(Messages.Deleted);
        }

        public async Task<IDataResult<IEnumerable<BlogListDto>>> GetAllBlogListByUserIdAsync()
        {
            var blogList = await _unitOfWork.BlogRepository.Table.Where(d => d.UserId == _requestContext.UserId).AsNoTracking().OrderByDescending(d => d.UpdateTime).Select(d => new BlogListDto
            {
                Id = d.Id,
                CreateTime = d.CreateTime,
                IsActive = d.IsActive,
                FirstVisibleImageName = d.FirstVisibleImageName,
                Title = d.Title,
                SeoUrl = d.SeoUrl
            }).ToListAsync();

            return new SuccessDataResult<IEnumerable<BlogListDto>>(blogList);
        }

        public async Task<IDataResult<IEnumerable<BlogListDto>>> GetAllBlogListByUserNameAsync(string userName)
        {
            var blogList = await _unitOfWork.BlogRepository.Include(d => d.User).Where(d => d.User.UserName == userName).AsNoTracking().Where(d => d.IsActive).OrderByDescending(d => d.UpdateTime).Select(d => new BlogListDto
            {
                Id = d.Id,
                UserName = d.User.UserName,
                IsActive = d.IsActive,
                UserSeoUrl = UrlHelper.FriendlyUrl(d.User.UserName),
                CreateTime = d.CreateTime,
                FirstVisibleImageName = d.FirstVisibleImageName,
                Title = d.Title,
                SeoUrl = d.SeoUrl
            }).ToListAsync();

            return new SuccessDataResult<IEnumerable<BlogListDto>>(blogList);
        }
        public async Task<IDataResult<IEnumerable<BlogListDto>>> GetAllBlogListAsync()
        {
            var blogList = await _unitOfWork.BlogRepository.Include(d => d.User)
                .AsNoTracking().OrderByDescending(d => d.UpdateTime).Select(d => new BlogListDto
                {
                    Id = d.Id,
                    UserName = d.User.UserName,
                    IsActive = d.IsActive,
                    UserSeoUrl = UrlHelper.FriendlyUrl(d.User.UserName),
                    CreateTime = d.CreateTime,
                    FirstVisibleImageName = d.FirstVisibleImageName,
                    Title = d.Title,
                    SeoUrl = d.SeoUrl
                }).ToListAsync();

            return new SuccessDataResult<IEnumerable<BlogListDto>>(blogList);
        }
        public async Task<IDataResult<IEnumerable<BlogListDto>>> GetAllBlogViewListAsync(int count)
        {
           
            if(count != 0)
            {
                var blogListWithCount = await _unitOfWork.BlogRepository.Include(d => d.User).Where(d => d.IsActive)
               .AsNoTracking().OrderByDescending(d => d.UpdateTime).Take(count).Select(d => new BlogListDto
               {
                   Id = d.Id,
                   UserName = d.User.UserName,
                   IsActive = d.IsActive,
                   UserSeoUrl = UrlHelper.FriendlyUrl(d.User.UserName),
                   CreateTime = d.CreateTime,
                   FirstVisibleImageName = d.FirstVisibleImageName,
                   Title = d.Title,
                   SeoUrl = d.SeoUrl
               }).ToListAsync();
                return new SuccessDataResult<IEnumerable<BlogListDto>>(blogListWithCount);
            }
            var blogList = await _unitOfWork.BlogRepository.Include(d => d.User).Where(d => d.IsActive)
               .AsNoTracking().OrderByDescending(d => d.UpdateTime).Select(d => new BlogListDto
               {
                   Id = d.Id,
                   UserName = d.User.UserName,
                   IsActive = d.IsActive,
                   UserSeoUrl = UrlHelper.FriendlyUrl(d.User.UserName),
                   CreateTime = d.CreateTime,
                   FirstVisibleImageName = d.FirstVisibleImageName,
                   Title = d.Title,
                   SeoUrl = d.SeoUrl
               }).ToListAsync();
            return new SuccessDataResult<IEnumerable<BlogListDto>>(blogList);
        }

        public async Task<IDataResult<BlogDetailDto>> GetBlogDetailByIdAsync(int Id)
        {
            var blogDetailDto = await _unitOfWork.BlogRepository.Include(d => d.BlogItems, u => u.User).Where(d => d.Id == Id).Select(d => new BlogDetailDto
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
            }).FirstOrDefaultAsync();

            if (blogDetailDto == null)
                return new ErrorDataResult<BlogDetailDto>(Messages.ObjectIsNull);
            return new SuccessDataResult<BlogDetailDto>(blogDetailDto);
        }

        public async Task<IDataResult<BlogDetailDto>> GetBlogDetailBySeoUrlAsync(string seoUrl)
        {
            var blogDetailDto = await _unitOfWork.BlogRepository.Include(d => d.BlogItems, u => u.User).Where(d => d.SeoUrl == seoUrl && d.IsActive).Select(d => new BlogDetailDto
            {
                Id = d.Id,
                Author = d.User.UserName,
                CreateTime = d.CreateTime,
                Title = d.Title,
                FirstVisibleImageName = d.FirstVisibleImageName,
                MetaDescription = d.MetaDescription,
                MetaKeywords = d.MetaKeywords,
                MetaTitle = d.MetaTitle,
                BlogItems = d.BlogItems.Select(b => new BlogItemDto
                {
                    Id = b.Id,
                    Description = b.Description,
                    ImageName = b.ImageName
                }).ToList()
            }).FirstOrDefaultAsync();

            if (blogDetailDto == null)
                return new ErrorDataResult<BlogDetailDto>(Messages.ObjectIsNull);
            return new SuccessDataResult<BlogDetailDto>(blogDetailDto);
        }

        public async Task<IDataResult<BlogUpdateDto>> GetBlogUpdateBySeoUrlAsync(string seoUrl)
        {
            var blogUpdateDto = await _unitOfWork.BlogRepository.Include(d => d.BlogItems).Where(d => d.SeoUrl == seoUrl && ((_requestContext.Roles.Contains(GeneralConstants.Admin) || _requestContext.Roles.Contains(GeneralConstants.Editor)) ? true : d.UserId == _requestContext.UserId)).Select(d => new BlogUpdateDto
            {
                Id = d.Id,
                Title = d.Title,
                FirstVisibleImageName = d.FirstVisibleImageName,
                MetaDescription = d.MetaDescription,
                MetaKeywords = d.MetaKeywords,
                MetaTitle = d.MetaTitle,
                BlogItems = d.BlogItems.Select(b => new BlogItemDto
                {
                    Id = b.Id,
                    Description = b.Description,
                    ImageName = b.ImageName
                }).ToList()
            }).FirstOrDefaultAsync();

            if (blogUpdateDto == null)
                return new ErrorDataResult<BlogUpdateDto>(Messages.ObjectIsNull);

            return new SuccessDataResult<BlogUpdateDto>(blogUpdateDto);
        }

        [ValidationAspect(typeof(BlogInsertDtoValidator), Priority = 1)]
        public async Task<IDataResult<int>> InsertBlogAsync(BlogInsertDto blogInsertDto)
        {
            var seoUrl = UrlHelper.FriendlyUrl(blogInsertDto.Title);
            var savedSeoUrl = await _unitOfWork.BlogRepository.Table.Where(d => d.SeoUrl == seoUrl).Select(d => d.SeoUrl).AsNoTracking().FirstOrDefaultAsync();

            if (savedSeoUrl != null)
            {
                seoUrl += Guid.NewGuid().ToString().Substring(0, 5);
            }

            var blog = new Blog()
            {
                UserId = _requestContext.UserId,
                Title = blogInsertDto.Title,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                FirstVisibleImageName = blogInsertDto.FirstVisibleImageName,
                SeoUrl = seoUrl,
                MetaDescription = blogInsertDto.MetaDescription,
                MetaKeywords = blogInsertDto.MetaKeywords,
                MetaTitle = blogInsertDto.MetaTitle,
                IsActive = false
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
            await _unitOfWork.SaveChangesAsync();

            return new SuccessDataResult<int>(blog.Id);
        }

        [ValidationAspect(typeof(BlogUpdateDtoValidator), Priority = 1)]
        public async Task<IDataResult<int>> UpdateBlogAsync(BlogUpdateDto blogUpdateDto)
        {

            var blog = await _unitOfWork.BlogRepository.Include(d => d.BlogItems).Where(d => d.Id == blogUpdateDto.Id && ((_requestContext.Roles.Contains(GeneralConstants.Admin) || _requestContext.Roles.Contains(GeneralConstants.Editor)) ? true : d.UserId == _requestContext.UserId)).FirstOrDefaultAsync();

            if (blog == null)
                return new ErrorDataResult<int>(Messages.ObjectIsNull);

            blog.Title = blogUpdateDto.Title;
            var seoUrl = UrlHelper.FriendlyUrl(blogUpdateDto.Title);

            if (seoUrl != blog.SeoUrl)
            {
                string savedSeoUrl = await _unitOfWork.BlogRepository.Table.Where(d => d.SeoUrl == seoUrl).Select(d => d.SeoUrl).FirstOrDefaultAsync();

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
            blog.IsActive = false;
            blog.FirstVisibleImageName = blogUpdateDto.FirstVisibleImageName;
            blog.MetaTitle = blogUpdateDto.MetaTitle;
            blog.MetaKeywords = blogUpdateDto.MetaKeywords;
            blog.MetaDescription = blogUpdateDto.MetaDescription;

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
            await _unitOfWork.SaveChangesAsync();
            return new SuccessDataResult<int>(blog.Id);
        }

        public async Task<IResult> UpdateBlogActivateAsync(int blogId, bool isActive)
        {
            var blog = await _unitOfWork.BlogRepository.GetByIdAsync(blogId);
            if (blog == null)
                return new ErrorResult(Messages.ObjectIsNull);
            blog.IsActive = isActive;
            await _unitOfWork.SaveChangesAsync();

            return new SuccessResult(Messages.Updated);
        }
    }
}
