    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Data;

namespace SE.Business.AttributeServices
{
    public class AttributeService : IAttributeService
    {
        private readonly IRepository<Core.Entities.Attribute> _attributeRepo;
        private readonly IRepository<AttributeCategory> _attributeCategoryRepo;
        private readonly IRepository<CategoryAttributeCategory> _categoryAttributeCategoryRepo;

        public AttributeService(IRepository<Core.Entities.Attribute> attributeRepo, IRepository<AttributeCategory> attributeCategoryRepo, IRepository<CategoryAttributeCategory> categoryAttributeCategoryRepo)
        {
            _attributeRepo = attributeRepo;
            _attributeCategoryRepo = attributeCategoryRepo;
            _categoryAttributeCategoryRepo = categoryAttributeCategoryRepo;
        }

        public List<AttributeListDto> GetAllAttributeByEducationCategoryId(int categoryId)
        {
            try
            {
                var attributeCategoryList = _categoryAttributeCategoryRepo.Table.Where(d => d.CategoryId == categoryId).Select(d=>d.AttributeCategoryId).ToList();

                var educationAttributeList = (from r in _attributeRepo.Table
                                              join s in _attributeCategoryRepo.Table
                                              on r.AttributeCategoryId equals s.Id
                                              where attributeCategoryList.Contains(s.Id)
                                              group new { r, s } by new { s.Name }
                             into grp
                                              select new AttributeListDto
                                              {
                                                  CategoryName = grp.Key.Name,
                                                  AttributeDtoList = grp.Select(d => new AttributeDto
                                                  {
                                                      Id = d.r.Id,
                                                      Name = d.r.Name
                                                  }).ToList()
                                              }).ToList();
                return educationAttributeList;
            }
            catch 
            {
                throw; 
            }
        }
    }
}
