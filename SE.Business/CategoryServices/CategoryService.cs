using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE.Business.Helpers;
using SE.Core.DTO;
using SE.Core.Entities;
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

        public void DeleteCategory(int categoryId)
        {
            try
            {
                var category = _unitOfWork.CategoryRepository.GetById(categoryId);
                _unitOfWork.CategoryRepository.Delete(category);
                _unitOfWork.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public List<CategoryDto> GetAllCategoryList()
        {
            try
            {
                var categoryList = _unitOfWork.CategoryRepository.Table.Select(d => new CategoryDto { Name = d.Name, Id = d.Id, SeoUrl = UrlHelper.FriendlyUrl(d.Name) }).ToList();
                return categoryList;
            }
            catch
            {
                throw;
            }
        }

        public CategoryDto GetCategoryById(int categoryId)
        {
            try
            {
                var categoryDto = _unitOfWork.CategoryRepository.Table.Where(d => d.Id == categoryId).Select(d => new CategoryDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    SeoUrl = d.SeoUrl
                }).FirstOrDefault();

                if (categoryDto != null)
                {
                    return categoryDto;
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

        public void InsertCategory(CategoryDto categoryDto)
        {
            try
            {
                var category = new Category
                {
                    Name = categoryDto.Name,
                    SeoUrl = UrlHelper.FriendlyUrl(categoryDto.Name)
                };
                _unitOfWork.CategoryRepository.Insert(category);
                _unitOfWork.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void UpdateCategory(CategoryDto categoryDto)
        {
            try
            {
                var category = _unitOfWork.CategoryRepository.GetById(categoryDto.Id);
                category.Name = categoryDto.Name;
                category.SeoUrl = UrlHelper.FriendlyUrl(categoryDto.Name);
                _unitOfWork.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
