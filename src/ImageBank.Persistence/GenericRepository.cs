using System;
using System.Data.Entity;
using System.Linq;

namespace ImageBank.Persistence
{
    public abstract class GenericRepository<TContext, TEntity, TKey> : IGenericRepository<TEntity, TKey>, IDisposable where TContext : DbContext where TEntity : class where TKey : struct
    {
        public TContext Context { get; private set; }

        protected GenericRepository(TContext ctx)
        {
            Context = ctx;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            return query;
        }

        public virtual TEntity Get(TKey id)
        {
            var query = Context.Set<TEntity>().Find(id);
            return query;
        }

        public IQueryable<TEntity> FindBy(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>().Where(predicate);
            return query;
        }

        public IQueryable<TEntity> Include(string include)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>().Include(include);
            return query;
        }

        public virtual void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(TKey id)
        {
            var entity = Get(id);
            Context.Set<TEntity>().Remove(entity);
            Context.SaveChanges();
        }

        public virtual void Edit(TEntity entity)
        {
            Context.Entry(entity).State = System.Data.EntityState.Modified;
            Context.SaveChanges();
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            //if (Context != null)
            //{
            //    try
            //    {
            //        Context.Dispose();
            //    }
            //    catch(Exception ex)
            //    {

            //    }
            //}
        }
    }
}