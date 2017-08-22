using Model;
using Repository;
using System.Collections.Generic;

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

        public IList<Ship> GetByPlanet(int planetId)
        {
            return repository.List<Ship>(x => x.Planet.Id == planetId);
        }
    }
}
