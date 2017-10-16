using System;
using System.Collections.Generic;
using Model;
using Repository;

namespace Service.Defenses
{
    public class DefenseService : IDefenseService
    {
        private readonly IRepositoryService repository;

        public DefenseService(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public IList<Canon> GetCanons(int planetId)
        {
            return repository.List<Canon>(x => x.Planet.Id == planetId && x.EnableDate < DateTime.Now);
        }

        public Shield GetShieldStatus(int planetId)
        {
            Shield shield = repository.Get<Shield>(x => x.Planet.Id == planetId && x.EnableDate < DateTime.Now);

            return (shield);
        }
    }
}
