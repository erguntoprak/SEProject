using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SE.Data
{
    public interface IRepository<TEntity> where TEntity:BaseEntity
    {
        TEntity GetById(object id);
        Task<TEntity> GetByIdAsync(object id);
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> expression = null);
        Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression = null);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Table { get; }
        IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includes);
    }
}
