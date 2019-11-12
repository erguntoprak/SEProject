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
            var educationAttributeList = (from r in _educationAttributeRepo.Table
                                          join s in _educationAttributeCategoryRepo.Table
                                          on r.EducationAttributeCategoryId equals s.Id
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
    }
}
