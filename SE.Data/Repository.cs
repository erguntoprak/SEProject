using Microsoft.EntityFrameworkCore;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SE.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        private DbSet<TEntity> _entities;

        public TEntity GetById(object id)
        {
            return Entities.Find(id);
        }
        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await Entities.FindAsync(id);
        }
        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> expression = null)
        {
            return expression == null ? Entities.AsNoTracking() : Entities.Where(expression).AsNoTracking();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            return expression == null ? await Entities.ToListAsync() :
                 await Entities.Where(expression).ToListAsync();
        }
        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Entities.Remove(entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            Entities.RemoveRange(entities);
        }

        public void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Entities.Add(entity);
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Entities.Update(entity);
        }

        public IQueryable<TEntity> Include(Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Entities;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query ?? Entities;
        }

        public IQueryable<TEntity> Table => Entities;

        protected DbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<TEntity>();
                return _entities;
            }
        }
    }
}
