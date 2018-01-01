using System;
using Model;
using Repository;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Timers;
using Model.Enum;

namespace Service.Ships
{
    public class ShipService : IShipService
    {
        private readonly IRepositoryService repository;

        public ShipService(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public Ship Get(int id)
        {
            return repository.Get<Ship>(id);
        }

        public IList<Ship> GetAvailableByPlanet(int planetId)
        {
            var attacks = repository.List<Attack>(x => x.AttackerPlanet.Id == planetId && (x.FleetArrival < DateTime.Now
                || DbFunctions.DiffSeconds(x.FleetArrival, x.FleetDeparture) > DbFunctions.DiffSeconds(DateTime.Now, x.FleetArrival)));
            var attackingShipsIds = attacks.SelectMany(x => x.Fleet.Select(y => y.Id));

            return repository.List<Ship>(x => x.Planet.Id == planetId && x.EnableDate < DateTime.Now && !attackingShipsIds.Contains(x.Id)
                && (x.ShipType == ShipType.X || x.ShipType == ShipType.Y || x.ShipType == ShipType.Z));
        }

        public IList<Ship> GetByPlanet(int planetId)
        {
            return repository.List<Ship>(x => x.Planet.Id == planetId && x.EnableDate < DateTime.Now
                && (x.ShipType == ShipType.X || x.ShipType == ShipType.Y || x.ShipType == ShipType.Z));
        }

        public IList<Ship> GetShipsCost()
        {
            var ships = new List<Ship>();

            var shipXCost = repository.Get<Cost>(x => x.Element == "ship x");
            var shipX = new ShipX
            {
                ShipType = ShipType.X,
                Attack = 100,
                Defence = 100,
                Speed = 10,
                ConstructionTime = 5,
                RequiredLevel = 2,
                Cost = shipXCost,
                DarkMatterConsumption = 10,
            };
            ships.Add(shipX);

            var shipYCost = repository.Get<Cost>(x => x.Element == "ship y");
            var shipY = new ShipY
            {
                ShipType = ShipType.Y,
                Attack = 150,
                Defence = 100,
                Speed = 15,
                ConstructionTime = 10,
                RequiredLevel = 3,
                Cost = shipYCost,
                DarkMatterConsumption = 15,
            };
            ships.Add(shipY);

            var shipZCost = repository.Get<Cost>(x => x.Element == "ship z");
            var shipZ = new ShipZ
            {
                ShipType = ShipType.Z,
                Attack = 300,
                Defence = 200,
                Speed = 20,
                ConstructionTime = 15,
                RequiredLevel = 5,
                Cost = shipZCost,
                DarkMatterConsumption = 30,
            };
            ships.Add(shipZ);

            return ships;
        }

        public void FinishShip(Timer timer, Ship ship)
        {
            //mandar notificacion push al usuario

            //elimino el timer
            timer.Stop();
            timer.Dispose();
        }

    }
}
