using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Construction
{
    public class ConstructionService : IConstructionService
    {
        private readonly IRepositoryService repository;

        public ConstructionService(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public IList<Mine> GetMinesToBuild(int planetId)
        {
            CrystalMine actualCrystalMine = repository.Get<CrystalMine>(x => x.Planet.Id == planetId)
            MetalMine actualCrystalMine = repository.Get<MetalMine>(x => x.Planet.Id == planetId)
            DarkMatterMine actualCrystalMine = repository.Get<DarkMatterMine>(x => x.Planet.Id == planetId)

            List<Mine> mines = new List<Mine>();
            mines.Add(repository.Get<CrystalMine>(x => x.Level == actualCrystalMine.Level + 1))
            mines.Add(repository.Get<MetalMine>(x => x.Level == actualMetalMine.Level + 1))
            mines.Add(repository.Get<DarkMatterMine>(x => x.Level == actualDarkMatterMine.Level + 1))

            return mines;
        }
    }
}
