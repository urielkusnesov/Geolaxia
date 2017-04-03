using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Ninject.Modules;
using Ninject.Web.Common;
using Model;
using Repository;
using Service.Players;

namespace Geolaxia.Dependencies
{
    public class NinjectWebModule : NinjectModule
    {
        public override void Load()
        {
            // Comunicacion Directa
            // Cuando se solicite una implementacion de IConfiguracionProvider o ConfiguracionProvider, ninject devolverá ConfiguracionProvider
            Bind<DbContext>().To<GeolaxiaContext>().InRequestScope();
            Bind<IRepositoryService, RepositoryService>().To<RepositoryService>().InRequestScope();
            Bind<IPlayerService, PlayerService>().To<PlayerService>().InRequestScope();
        }
    }
}