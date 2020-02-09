using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public List<CategoryDto> GetAllCategoryList()
        {
            try
            {
                var categoryList = _unitOfWork.CategoryRepository.Table.Select(d=> new CategoryDto { Name=d.Name, Id=d.Id }).ToList();
                return categoryList;
            }
            catch 
            {
                throw;
            }
        }
    }
}
