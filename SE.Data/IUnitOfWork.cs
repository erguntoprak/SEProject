﻿using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<City> CityRepository { get; }
        IRepository<District> DistrictRepository { get; }
        IRepository<Core.Entities.Attribute> AttributeRepository { get; }
        IRepository<AttributeCategory> AttributeCategoryRepository { get; }
        IRepository<CategoryAttributeCategory> CategoryAttributeCategoryRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IRepository<Education> EducationRepository { get; }
        IRepository<EducationAddress> AddressRepository { get; }
        IRepository<Image> ImageRepository { get; }
        IRepository<Question> QuestionRepository { get; }
        IRepository<AttributeEducation> AttributeEducationRepository { get; }
        IRepository<EducationContactForm> EducationContactFormRepository { get; }
        IRepository<Blog> BlogRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
