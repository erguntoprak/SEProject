using Microsoft.EntityFrameworkCore;
using SE.Business.Constants;
using SE.Core.Aspects.Autofac.Caching;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Core.Utilities.Results;
using SE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.Business.AttributeCategoryServices
{
    public class AttributeCategoryService : IAttributeCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AttributeCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAttributeCategoryAsync(int attributeCategoryId)
        {
            var attributeCategory = await _unitOfWork.AttributeCategoryRepository.GetByIdAsync(attributeCategoryId);
            if (attributeCategory == null)
                return new ErrorResult(Messages.ObjectIsNull);

            _unitOfWork.AttributeCategoryRepository.Delete(attributeCategory);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult(Messages.Deleted);
        }

        [CacheAspect]
        public async Task<IDataResult<IEnumerable<AttributeCategoryDto>>> GetAllAttributeCategoryListAsync()
        {
            var attributeCategoryList = await _unitOfWork.AttributeCategoryRepository.Table.Select(d => new AttributeCategoryDto { Name = d.Name, Id = d.Id }).ToListAsync();
            return new SuccessDataResult<IEnumerable<AttributeCategoryDto>>(attributeCategoryList);
        }

        public async Task<IDataResult<int[]>> GetAttributeCategoryIdsByCategoryIdAsync(int categoryId)
        {
            var attributeCategoryIds = await _unitOfWork.CategoryAttributeCategoryRepository.Table.Where(d => d.CategoryId == categoryId).Select(d => d.AttributeCategoryId).ToArrayAsync();
            return new SuccessDataResult<int[]>(attributeCategoryIds);
        }

        public async Task<IDataResult<AttributeCategoryDto>> GetAttributeCategoryByIdAsync(int attributeCategoryId)
        {
            var attributeCategoryDto = await _unitOfWork.AttributeCategoryRepository.Table.Where(d => d.Id == attributeCategoryId).Select(d => new AttributeCategoryDto
            {
                Id = d.Id,
                Name = d.Name
            }).FirstOrDefaultAsync();

            if (attributeCategoryDto == null)
                return new ErrorDataResult<AttributeCategoryDto>(Messages.ObjectIsNull);

            return new SuccessDataResult<AttributeCategoryDto>(attributeCategoryDto);
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> InsertAttributeCategoryAsync(AttributeCategoryDto attributeCategoryDto)
        {
            var attributeCategory = new AttributeCategory
            {
                Name = attributeCategoryDto.Name
            };
            _unitOfWork.AttributeCategoryRepository.Insert(attributeCategory);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult(Messages.Added);
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> InsertCategoryAttributeCategoryAsync(CategoryAttributeCategoryInsertDto categoryAttributeCategoryInsertDto)
        {
            var savedCategoryAttributeCategoryList = await _unitOfWork.CategoryAttributeCategoryRepository.Table.Where(d => d.CategoryId == categoryAttributeCategoryInsertDto.CategoryId).AsNoTracking().ToListAsync();

            _unitOfWork.CategoryAttributeCategoryRepository.Delete(savedCategoryAttributeCategoryList);

            foreach (var attributeCategory in categoryAttributeCategoryInsertDto.AttributeCategoryList)
            {
                _unitOfWork.CategoryAttributeCategoryRepository.Insert(new CategoryAttributeCategory { CategoryId = categoryAttributeCategoryInsertDto.CategoryId, AttributeCategoryId = attributeCategory.Id });
            }
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult(Messages.Added);
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> UpdateAttributeCategoryAsync(AttributeCategoryDto attributeCategoryDto)
        {
            var attributeCategory = await _unitOfWork.AttributeCategoryRepository.GetByIdAsync(attributeCategoryDto.Id);
            attributeCategory.Name = attributeCategoryDto.Name;
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult(Messages.Updated);
        }
    }
}
