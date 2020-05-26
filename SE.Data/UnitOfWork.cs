using Microsoft.EntityFrameworkCore;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private Repository<City> _cityRepo;
        private Repository<District> _districtRepo;
        private Repository<Core.Entities.Attribute> _attributeRepo;
        private Repository<AttributeCategory> _attributeCategoryRepo;
        private Repository<CategoryAttributeCategory> _categoryAttributeCategoryRepo;
        private Repository<Category> _categoryRepo;
        private Repository<Education> _educationRepo;
        private Repository<EducationAddress> _addressRepo; 
        private Repository<Image> _imageRepo;
        private Repository<Question> _questionRepo;
        private Repository<AttributeEducation> _attributeEducationRepo;
        private Repository<EducationContactForm> _educationContactFormRepo;
        private Repository<Blog> _blogRepo;


        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<City> CityRepository
        {
            get
            {
                if (_cityRepo == null)
                    _cityRepo = new Repository<City>(_dbContext);
                return _cityRepo;
            }
        }
        public IRepository<District> DistrictRepository
        {
            get
            {
                if (_districtRepo == null)
                    _districtRepo = new Repository<District>(_dbContext);
                return _districtRepo;
            }
        }
        public IRepository<Core.Entities.Attribute> AttributeRepository
        {
            get
            {
                if (_attributeRepo == null)
                    _attributeRepo = new Repository<Core.Entities.Attribute>(_dbContext);
                return _attributeRepo;
            }
        }
        public IRepository<AttributeCategory> AttributeCategoryRepository
        {
            get
            {
                if (_attributeCategoryRepo == null)
                    _attributeCategoryRepo = new Repository<AttributeCategory>(_dbContext);
                return _attributeCategoryRepo;
            }
        }
        public IRepository<CategoryAttributeCategory> CategoryAttributeCategoryRepository
        {
            get
            {
                if (_categoryAttributeCategoryRepo == null)
                    _categoryAttributeCategoryRepo = new Repository<CategoryAttributeCategory>(_dbContext);
                return _categoryAttributeCategoryRepo;
            }
        }
        public IRepository<Category> CategoryRepository
        {
            get
            {
                if (_categoryRepo == null)
                    _categoryRepo = new Repository<Category>(_dbContext);
                return _categoryRepo;
            }
        }
        public IRepository<Education> EducationRepository
        {
            get
            {
                if (_educationRepo == null)
                    _educationRepo = new Repository<Education>(_dbContext);
                return _educationRepo;
            }
        }
        public IRepository<EducationAddress> AddressRepository
        {
            get
            {
                if (_addressRepo == null)
                    _addressRepo = new Repository<EducationAddress>(_dbContext);
                return _addressRepo;
            }
        }
        public IRepository<Image> ImageRepository
        {
            get
            {
                if (_imageRepo == null)
                    _imageRepo = new Repository<Image>(_dbContext);
                return _imageRepo;
            }
        }
        public IRepository<Question> QuestionRepository
        {
            get
            {
                if (_questionRepo == null)
                    _questionRepo = new Repository<Question>(_dbContext);
                return _questionRepo;
            }
        }
        public IRepository<AttributeEducation> AttributeEducationRepository
        {
            get
            {
                if (_attributeEducationRepo == null)
                    _attributeEducationRepo = new Repository<AttributeEducation>(_dbContext);
                return _attributeEducationRepo;
            }
        }
        public IRepository<EducationContactForm> EducationContactFormRepository
        {
            get
            {
                if (_educationContactFormRepo == null)
                    _educationContactFormRepo = new Repository<EducationContactForm>(_dbContext);
                return _educationContactFormRepo;
            }
        }

        public IRepository<Blog> BlogRepository
        {
            get
            {
                if (_blogRepo == null)
                    _blogRepo = new Repository<Blog>(_dbContext);
                return _blogRepo;
            }
        }

        public void SaveChanges()
        {

            try
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }


        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}