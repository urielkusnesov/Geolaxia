using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace Repository
{
    public class RepositoryService : IRepositoryService
    {
        private readonly DbContext context;

        public RepositoryService(DbContext context)
        {
            this.context = context;
        }

        private IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return context.Set<TEntity>();
        }

        public TEntity Get<TEntity>(object id) where TEntity : class
        {
            return context.Set<TEntity>().Find(id);
        }

        public TEntity Get<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            return context.Set<TEntity>().SingleOrDefault(filter);
        }

        public IList<TEntity> List<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            IQueryable<TEntity> result = context.Set<TEntity>();
            if (filter != null)
            {
                result = result.Where(filter);
            }
            return result.ToList();
        }

        public int Count<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            return filter != null ? Set<TEntity>().Count(filter) : Set<TEntity>().Count();
        }

        public bool Exists<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            return Set<TEntity>().Where(filter).Any();
        }

        public TEntity Add<TEntity>(TEntity entidad) where TEntity : class
        {
            return Set<TEntity>().Add(entidad);
        }

        public TEntity Remove<TEntity>(TEntity entidad) where TEntity : class
        {
            return Set<TEntity>().Remove(Get<TEntity>(entidad));
        }

        public TEntity Remove<TEntity>(object id) where TEntity : class
        {
            return Set<TEntity>().Remove(Get<TEntity>(id));
        }

        public IList<int> ExecuteListQuery(string sql)
        {
            return context.Database.SqlQuery<int>(sql).ToList();
        }

        public TEntity Max<TEntity, TOrder>(Expression<Func<TEntity, bool>> filtro, Expression<Func<TEntity, TOrder>> orderColumn)
            where TEntity : class
            where TOrder : IComparable
        {
            return Set<TEntity>().Where(filtro).OrderByDescending(orderColumn).FirstOrDefault();
        }

        public TEntity Min<TEntity, TOrder>(Expression<Func<TEntity, bool>> filtro, Expression<Func<TEntity, TOrder>> orderColumn)
            where TEntity : class
            where TOrder : IComparable
        {
            return Set<TEntity>().Where(filtro).OrderBy(orderColumn).FirstOrDefault();
        }

        public int SaveChanges()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
