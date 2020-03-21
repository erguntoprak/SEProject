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

        private readonly IUnitOfWork _unitOfWork;
        public AttributeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<CategoryAttributeListDto> GetAllAttributeByEducationCategoryId(int categoryId)
        {
            try
            {
                var attributeCategoryList = _unitOfWork.CategoryAttributeCategoryRepository.Table.Where(d => d.CategoryId == categoryId).Select(d => d.AttributeCategoryId).ToList();

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



                return educationAttributeGroupList;
            }
            catch
            {
                throw;
            }
        }
    }
}
