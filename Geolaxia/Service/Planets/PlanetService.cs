using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Planets
{
    public class PlanetService : IPlanetService
    {
        private readonly IRepositoryService repository;

        public PlanetService(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public Planet Get(int id)
        {
            return repository.Get<Planet>(id);
        }

        public IList<Planet> GetByPlayer(string username)
        {
            var planets = repository.List<Planet>(x => x.Conqueror.UserName == username);
            return planets;
        }
    }
}
