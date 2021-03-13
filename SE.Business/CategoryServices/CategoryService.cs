using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SE.Business.Constants;
using SE.Business.Helpers;
using SE.Core.Aspects.Autofac.Caching;
using SE.Core.Aspects.Autofac.Logging;
using SE.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Core.Utilities.Results;
using SE.Data;

namespace SE.Business.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteCategoryAsync(int categoryId)
        {

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                return new ErrorResult(Messages.ObjectIsNull);

            _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult(Messages.Deleted);
        }

        [CacheAspect]
        public async Task<IDataResult<IEnumerable<CategoryDto>>> GetAllCategoryListAsync()
        {
            var categoryList = await _unitOfWork.CategoryRepository.Table.AsNoTracking().Select(d => new CategoryDto { Name = d.Name, Id = d.Id, SeoUrl = UrlHelper.FriendlyUrl(d.Name) }).OrderBy(d=>d.Id).ToListAsync();

            return new SuccessDataResult<IEnumerable<CategoryDto>>(categoryList);
        }

        public async Task<IDataResult<CategoryDto>> GetCategoryByIdAsync(int categoryId)
        {

            var categoryDto = await _unitOfWork.CategoryRepository.Table.Where(d => d.Id == categoryId).Select(d => new CategoryDto
            {
                Id = d.Id,
                Name = d.Name,
                SeoUrl = d.SeoUrl
            }).FirstOrDefaultAsync();

            if (categoryDto == null)
                return new ErrorDataResult<CategoryDto>(Messages.ObjectIsNull);

            return new SuccessDataResult<CategoryDto>(categoryDto);
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> InsertCategoryAsync(CategoryDto categoryDto)
        {

            var category = new Category
            {
                Name = categoryDto.Name,
                SeoUrl = UrlHelper.FriendlyUrl(categoryDto.Name)
            };
            _unitOfWork.CategoryRepository.Insert(category);
            await _unitOfWork.SaveChangesAsync();

            return new SuccessResult(Messages.Added);
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryDto.Id);
            if (category == null)
                return new ErrorResult(Messages.ObjectIsNull);

            category.Name = categoryDto.Name;
            category.SeoUrl = UrlHelper.FriendlyUrl(categoryDto.Name);
            await _unitOfWork.SaveChangesAsync();

            return new SuccessResult(Messages.Updated);
        }
    }
}
