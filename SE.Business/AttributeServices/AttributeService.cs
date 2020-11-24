using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SE.Business.Constants;
using SE.Core.Aspects.Autofac.Caching;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Core.Utilities.Results;
using SE.Data;

namespace SE.Business.AttributeServices
{
    public class AttributeService : IAttributeService
    {

        private readonly IUnitOfWork _unitOfWork;
        public AttributeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAttributeAsync(int attributeId)
        {
            var attribute = await _unitOfWork.AttributeRepository.GetByIdAsync(attributeId);
            if (attribute == null)
                return new ErrorResult(Messages.ObjectIsNull);

            _unitOfWork.AttributeRepository.Delete(attribute);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult(Messages.Deleted);
        }

        public async Task<IDataResult<IEnumerable<CategoryAttributeListDto>>> GetAllAttributeByEducationCategoryIdAsync(int categoryId)
        {
            var attributeCategoryList = await _unitOfWork.CategoryAttributeCategoryRepository.Table.Where(d => d.CategoryId == categoryId).Select(d => d.AttributeCategoryId).ToListAsync();

            var educationAttributeList = (from r in _unitOfWork.AttributeRepository.Table
                                          join s in _unitOfWork.AttributeCategoryRepository.Table
                                          on r.AttributeCategoryId equals s.Id
                                          where attributeCategoryList.Contains(s.Id)
                                          select r);

            var educationAttributeGroupList = educationAttributeList.AsEnumerable().GroupBy(d => d.AttributeCategoryId).Select(d => new CategoryAttributeListDto
            {
                CategoryName = _unitOfWork.AttributeCategoryRepository.Table.Where(x => x.Id == d.Key).FirstOrDefault().Name,
                AttributeListDto = d.Select(x => new AttributeDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()
            }).ToList();

            return new SuccessDataResult<IEnumerable<CategoryAttributeListDto>>(educationAttributeGroupList);
        }

        [CacheAspect]
        public async Task<IDataResult<IEnumerable<AttributeListDto>>> GetAllAttributeListAsync()
        {

            var attributeListDto = await _unitOfWork.AttributeRepository.Include(d => d.AttributeCategory).Select(d => new AttributeListDto { Id = d.Id, Name = d.Name, AttributeCategoryId = d.AttributeCategoryId, AttributeCategoryName = d.AttributeCategory.Name }).OrderBy(d => d.AttributeCategoryName).ToListAsync();

            return new SuccessDataResult<IEnumerable<AttributeListDto>>(attributeListDto);
        }

        public async Task<IDataResult<AttributeDto>> GetAttributeByIdAsync(int attributeId)
        {
            var attributeDto = await _unitOfWork.AttributeRepository.Table.Where(d => d.Id == attributeId).Select(d => new AttributeDto
            {
                Id = d.Id,
                Name = d.Name,
                AttributeCategoryId = d.AttributeCategoryId
            }).FirstOrDefaultAsync();

            if (attributeDto == null)
                return new ErrorDataResult<AttributeDto>(Messages.ObjectIsNull);

            return new SuccessDataResult<AttributeDto>(attributeDto);
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> InsertAttributeAsync(AttributeDto attributeDto)
        {
            var attribute = new Core.Entities.Attribute
            {
                Name = attributeDto.Name,
                AttributeCategoryId = attributeDto.AttributeCategoryId
            };

            _unitOfWork.AttributeRepository.Insert(attribute);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult(Messages.Added);
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> UpdateAttributeAsync(AttributeDto attributeDto)
        {
            var attribute = await _unitOfWork.AttributeRepository.GetByIdAsync(attributeDto.Id);
            if (attribute == null)
                return new ErrorResult(Messages.ObjectIsNull);

            attribute.Name = attributeDto.Name;
            attribute.AttributeCategoryId = attributeDto.AttributeCategoryId;
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult(Messages.Updated);
        }
    }
}
