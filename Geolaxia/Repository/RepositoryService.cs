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

        private IDbSet<TEntidad> Set<TEntidad>() where TEntidad : class
        {
            return context.Set<TEntidad>();
        }

        public TEntidad Get<TEntidad>(object id) where TEntidad : class
        {
            return context.Set<TEntidad>().Find(id);
        }

        public TEntidad Get<TEntidad>(Expression<Func<TEntidad, bool>> filter) where TEntidad : class
        {
            return context.Set<TEntidad>().SingleOrDefault(filter);
        }

        public IList<TEntidad> List<TEntidad>(Expression<Func<TEntidad, bool>> filter = null) where TEntidad : class
        {
            IQueryable<TEntidad> result = context.Set<TEntidad>();
            if (filter != null)
            {
                result = result.Where(filter);
            }
            return result.ToList();
        }

        public int Count<TEntidad>(Expression<Func<TEntidad, bool>> filter = null) where TEntidad : class
        {
            return filter != null ? Set<TEntidad>().Count(filter) : Set<TEntidad>().Count();
        }

        public bool Exists<TEntidad>(Expression<Func<TEntidad, bool>> filter) where TEntidad : class
        {
            return Set<TEntidad>().Where(filter).Any();
        }

        public TEntidad Add<TEntidad>(TEntidad entidad) where TEntidad : class
        {
            return Set<TEntidad>().Add(entidad);
        }

        public TEntidad Remove<TEntidad>(TEntidad entidad) where TEntidad : class
        {
            return Set<TEntidad>().Remove(Get<TEntidad>(entidad));
        }

        public TEntidad Remove<TEntidad>(object id) where TEntidad : class
        {
            return Set<TEntidad>().Remove(Get<TEntidad>(id));
        }

        public int SaveChanges()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
