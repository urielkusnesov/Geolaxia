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
            return repository.List<Canon>(x => x.Planet.Id == planetId);
        }
    }
}
