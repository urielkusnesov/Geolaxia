using System.Data.Entity;
using Ninject.Modules;
using Ninject.Web.Common;
using Model;
using Repository;
using Service.Attacks;
using Service.Defenses;
using Service.Planets;
using Service.Players;
using Service.Ships;
using Service.Construction;
using Service.Colonize;

namespace Geolaxia.Dependencies
{
    public class NinjectWebModule : NinjectModule
    {
        public override void Load()
        {
            // Comunicacion Directa
            // Cuando se solicite una implementacion de IRepositoryService o RepositoryService, ninject devolverá RepositoryService
            Bind<DbContext>().To<GeolaxiaContext>().InRequestScope();
            Bind<IRepositoryService, RepositoryService>().To<RepositoryService>().InRequestScope();
            Bind<IPlayerService, PlayerService>().To<PlayerService>().InRequestScope();
            Bind<IPlanetService, PlanetService>().To<PlanetService>().InRequestScope();
            Bind<IShipService, ShipService>().To<ShipService>().InRequestScope();
            Bind<IAttackService, AttackService>().To<AttackService>().InRequestScope();
            Bind<IDefenseService, DefenseService>().To<DefenseService>().InRequestScope();
            Bind<IColonizeService, ColonizeService>().To<ColonizeService>().InRequestScope();
            Bind<IConstructionService, ConstructionService>().To<ConstructionService>().InRequestScope();
        }
    }
}