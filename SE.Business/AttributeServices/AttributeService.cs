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
        private readonly IRepository<EducationAttribute> _educationAttributeRepo;
        private readonly IRepository<EducationAttributeCategory> _educationAttributeCategoryRepo;

        public AttributeService(IRepository<EducationAttribute> educationAttributeRepo, IRepository<EducationAttributeCategory> educationAttributeCategoryRepo)
        {
            _educationAttributeRepo = educationAttributeRepo;
            _educationAttributeCategoryRepo = educationAttributeCategoryRepo;
        }

        public List<AttributeListDto> GetAllEducationAttributeList()
        {
            var educationAttributeList = _educationAttributeRepo.Include(d => d.EducationAttributeCategory).Select(d => new AttributeListDto
            {
                Id = d.Id,
                Name = d.Name,
                CategoryName = d.EducationAttributeCategory.Name
            }).ToList();

            return educationAttributeList;
        }
    }
}
