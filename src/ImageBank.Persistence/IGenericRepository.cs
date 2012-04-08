using System;
using System.Linq;
using System.Linq.Expressions;

namespace ImageBank.Persistence
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Include(string includes);
        TEntity Get(int id);
        void Add(TEntity entity);
        void Delete(int id);
        void Edit(TEntity entity);
        void Save();
    }
}