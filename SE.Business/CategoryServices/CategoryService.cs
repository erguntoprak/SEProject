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
        private readonly IRepository<Category> _categoryRepo;
        public CategoryService(IRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        public List<CategoryDto> GetAllCategoryList()
        {
            try
            {
                var categoryList = _categoryRepo.Table.Select(d=> new CategoryDto { Name=d.Name, Id=d.Id }).ToList();
                return categoryList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
