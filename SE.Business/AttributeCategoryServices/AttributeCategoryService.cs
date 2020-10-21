using SE.Core.DTO;
using SE.Core.Entities;
using SE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.Business.AttributeCategoryServices
{
    public class AttributeCategoryService : IAttributeCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AttributeCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void DeleteAttributeCategory(int attributeCategoryId)
        {
            try
            {
                var attributeCategory = _unitOfWork.AttributeCategoryRepository.GetById(attributeCategoryId);
                _unitOfWork.AttributeCategoryRepository.Delete(attributeCategory);
                _unitOfWork.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public List<AttributeCategoryDto> GetAllAttributeCategoryList()
        {
            try
            {
                var attributeCategoryList = _unitOfWork.AttributeCategoryRepository.Table.Select(d => new AttributeCategoryDto { Name = d.Name, Id = d.Id }).ToList();
                return attributeCategoryList;
            }
            catch
            {
                throw;
            }
        }

        public int[] GetAttributeCategoryIdsByCategoryId(int categoryId)
        {
            try
            {
                var attributeCategoryIds = _unitOfWork.CategoryAttributeCategoryRepository.Table.Where(d => d.CategoryId == categoryId).Select(d => d.AttributeCategoryId).ToArray();
                return attributeCategoryIds;
            }
            catch
            {
                throw;
            }
        }

        public AttributeCategoryDto GetAttributeCategoryById(int attributeCategoryId)
        {
            try
            {
                var attributeCategoryDto = _unitOfWork.AttributeCategoryRepository.Table.Where(d => d.Id == attributeCategoryId).Select(d => new AttributeCategoryDto
                {
                    Id = d.Id,
                    Name = d.Name
                }).FirstOrDefault();

                if (attributeCategoryDto != null)
                {
                    return attributeCategoryDto;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw;
            }
        }

        public void InsertAttributeCategory(AttributeCategoryDto attributeCategoryDto)
        {
            try
            {
                var attributeCategory = new AttributeCategory
                {
                    Name = attributeCategoryDto.Name
                };
                _unitOfWork.AttributeCategoryRepository.Insert(attributeCategory);
                _unitOfWork.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void InsertCategoryAttributeCategory(CategoryAttributeCategoryInsertDto categoryAttributeCategoryInsertDto)
        {
            try
            {
                var savedCategoryAttributeCategoryList = _unitOfWork.CategoryAttributeCategoryRepository.Table.Where(d => d.CategoryId == categoryAttributeCategoryInsertDto.CategoryId).ToList();

                _unitOfWork.CategoryAttributeCategoryRepository.Delete(savedCategoryAttributeCategoryList);

                foreach (var attributeCategory in categoryAttributeCategoryInsertDto.AttributeCategoryList)
                {
                    _unitOfWork.CategoryAttributeCategoryRepository.Insert(new CategoryAttributeCategory {CategoryId = categoryAttributeCategoryInsertDto.CategoryId,AttributeCategoryId = attributeCategory.Id });
                }
                _unitOfWork.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void UpdateAttributeCategory(AttributeCategoryDto attributeCategoryDto)
        {
            try
            {
                var attributeCategory = _unitOfWork.AttributeCategoryRepository.GetById(attributeCategoryDto.Id);
                attributeCategory.Name = attributeCategoryDto.Name;
                _unitOfWork.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
