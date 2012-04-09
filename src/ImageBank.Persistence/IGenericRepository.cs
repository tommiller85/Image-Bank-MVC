using System;
using System.Linq;
using System.Linq.Expressions;

namespace ImageBank.Persistence
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : class where TKey : struct
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Include(string includes);
        TEntity Get(TKey id);
        void Add(TEntity entity);
        void Delete(TKey id);
        void Edit(TEntity entity);
        void Save();
    }
}