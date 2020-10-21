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

        public void DeleteAttribute(int attributeId)
        {
            try
            {
                var attribute = _unitOfWork.AttributeRepository.GetById(attributeId);
                _unitOfWork.AttributeRepository.Delete(attribute);
                _unitOfWork.SaveChanges();
            }
            catch
            {
                throw;
            }
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

        public List<AttributeListDto> GetAllAttributeList()
        {
            try
            {
                var attributeListDto = _unitOfWork.AttributeRepository.Include(d=>d.AttributeCategory).Select(d => new AttributeListDto { Id = d.Id, Name = d.Name,AttributeCategoryId = d.AttributeCategoryId,AttributeCategoryName = d.AttributeCategory.Name}).OrderBy(d=>d.AttributeCategoryName).ToList();
                return attributeListDto;
            }
            catch
            {
                throw;
            }
        }

        public AttributeDto GetAttributeById(int attributeId)
        {
            try
            {
                var attributeDto = _unitOfWork.AttributeRepository.Table.Where(d => d.Id == attributeId).Select(d => new AttributeDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    AttributeCategoryId = d.AttributeCategoryId
                }).FirstOrDefault();

                if (attributeDto != null)
                {
                    return attributeDto;
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

        public void InsertAttribute(AttributeDto attributeDto)
        {
            try
            {
                var attribute = new Core.Entities.Attribute
                {
                    Name = attributeDto.Name,
                    AttributeCategoryId = attributeDto.AttributeCategoryId
                };
                _unitOfWork.AttributeRepository.Insert(attribute);
                _unitOfWork.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void UpdateAttribute(AttributeDto attributeDto)
        {
            try
            {
                var attribute = _unitOfWork.AttributeRepository.GetById(attributeDto.Id);
                attribute.Name = attributeDto.Name;
                attribute.AttributeCategoryId = attributeDto.AttributeCategoryId;
                _unitOfWork.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
