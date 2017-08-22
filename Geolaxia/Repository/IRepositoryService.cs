using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository
{
    public interface IRepositoryService
    {
        TEntidad Get<TEntidad>(object id) where TEntidad : class;

        TEntidad Get<TEntidad>(Expression<Func<TEntidad, bool>> filter) where TEntidad : class;

        IList<TEntidad> List<TEntidad>(Expression<Func<TEntidad, bool>> filter = null) where TEntidad : class;

        int Count<TEntidad>(Expression<Func<TEntidad, bool>> filter = null) where TEntidad : class;

        bool Exists<TEntidad>(Expression<Func<TEntidad, bool>> filter) where TEntidad : class;

        TEntidad Add<TEntidad>(TEntidad entidad) where TEntidad : class;

        TEntidad Remove<TEntidad>(TEntidad entidad) where TEntidad : class;

        TEntidad Remove<TEntidad>(object id) where TEntidad : class;

        int SaveChanges();
    }
}
