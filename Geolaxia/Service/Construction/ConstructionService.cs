using Model;
using Repository;
using System.Collections.Generic;
using Model.Enum;

namespace Service.Construction
{
    public class ConstructionService : IConstructionService
    {
        private readonly IRepositoryService repository;

        public ConstructionService(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public IList<Mine> GetCurrentMines(int planetId)
        {
            var mines = new List<Mine>
            {
                repository.Get<CrystalMine>(x => x.Planet.Id == planetId) ?? new CrystalMine{MineType = MineType.Crystal},
                repository.Get<MetalMine>(x => x.Planet.Id == planetId) ?? new MetalMine{MineType = MineType.Metal},
                repository.Get<DarkMatterMine>(x => x.Planet.Id == planetId) ?? new DarkMatterMine{MineType = MineType.DarkMatter}
            };

            return mines;
        }

        
        public IList<Mine> GetMinesToBuild(int planetId)
        {
            var currentCrystalMineLevel = repository.Get<CrystalMine>(x => x.Planet.Id == planetId) != null ? repository.Get<CrystalMine>(x => x.Planet.Id == planetId).Level : 0;
            var currentMetalMineLevel = repository.Get<MetalMine>(x => x.Planet.Id == planetId) != null ? repository.Get<MetalMine>(x => x.Planet.Id == planetId).Level : 0;
            var currentDarkMatterMineLevel = repository.Get<DarkMatterMine>(x => x.Planet.Id == planetId) != null ? repository.Get<DarkMatterMine>(x => x.Planet.Id == planetId).Level : 0;

            var mines = new List<Mine>();
            
            var newCrystalMine = new CrystalMine {
                Cost = new Cost { CrystalCost = 100*(currentCrystalMineLevel + 1), MetalCost = 100*(currentCrystalMineLevel + 1) },
                EnergyConsumption = 50 * (currentCrystalMineLevel + 1),
                ConstructionTime = 1 + (20 * currentCrystalMineLevel),
                Productivity = 50 * (currentCrystalMineLevel + 1),
                MineType = MineType.Crystal,
                Level = currentCrystalMineLevel + 1
            };
            mines.Add(newCrystalMine);

            var newMetalMine = new MetalMine {
                Cost = new Cost { CrystalCost = 50*(currentMetalMineLevel + 2), MetalCost = 50*(currentMetalMineLevel + 2) },
                EnergyConsumption = 50 + (25 * currentMetalMineLevel),
                ConstructionTime = 1 + (10 * currentMetalMineLevel),
                Productivity = 50 * (currentMetalMineLevel + 1),
                MineType = MineType.Metal,
                Level = currentMetalMineLevel + 1
            };
            mines.Add(newMetalMine);

            var newDarkMatterMine = new DarkMatterMine
            {
                Cost = new Cost { CrystalCost = 100 + (200 * currentDarkMatterMineLevel), MetalCost = 100 + (200 * currentDarkMatterMineLevel) },
                EnergyConsumption = 100 + (200 * currentDarkMatterMineLevel),
                ConstructionTime = 1 + (30 * currentDarkMatterMineLevel),
                Productivity = 25 * (currentDarkMatterMineLevel + 1),
                MineType = MineType.DarkMatter,
                Level = currentDarkMatterMineLevel + 1
            };
            mines.Add(newDarkMatterMine);

            return mines;
        }

        public Mine Add(Mine mine)
        {
            var previousMine = repository.Get<Mine>(x => x.Planet.Id == mine.Planet.Id && x.MineType == mine.MineType);
            if (previousMine == null || repository.Remove(previousMine) != null)
            {
                var result = repository.Add(mine);
                repository.SaveChanges();
                return result;
            }
            return null;
        }
    }
}
