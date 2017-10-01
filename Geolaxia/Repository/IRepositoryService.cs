using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository
{
    public interface IRepositoryService
    {
        TEntity Get<TEntity>(object id) where TEntity : class;

        TEntity Get<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class;

        IList<TEntity> List<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class;

        int Count<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class;

        bool Exists<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class;

        TEntity Add<TEntity>(TEntity entidad) where TEntity : class;

        TEntity Remove<TEntity>(TEntity entidad) where TEntity : class;

        TEntity Remove<TEntity>(object id) where TEntity : class;

        TEntity Max<TEntity, TOrden>(Expression<Func<TEntity, bool>> filtro, Expression<Func<TEntity, TOrden>> columnaOrden)
            where TEntity : class
            where TOrden : IComparable;

        TEntity Min<TEntity, TOrden>(Expression<Func<TEntity, bool>> filtro, Expression<Func<TEntity, TOrden>> columnaOrden)
            where TEntity : class
            where TOrden : IComparable;

        int SaveChanges();

        IList<int> ExecuteListQuery(string sql);
    }
}
