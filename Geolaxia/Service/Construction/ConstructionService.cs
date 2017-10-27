using System;
using Model;
using Repository;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Model.DTO;
using Model.Enum;
using Service.Planets;

namespace Service.Construction
{
    public class ConstructionService : IConstructionService
    {
        private readonly IRepositoryService repository;
        private IPlanetService planetService;

        public ConstructionService(IRepositoryService repository, IPlanetService planetService)
        {
            this.repository = repository;
            this.planetService = planetService;
        }

        public IList<Mine> GetCurrentMines(int planetId)
        {
            var currentCrystalMine = repository.Max<CrystalMine, int>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now, x => x.Level)
                ?? new CrystalMine { MineType = MineType.Crystal, Planet = new BlackPlanet(), Cost = new Cost() };
            var inConstructionCrystalMine = repository.Get<CrystalMine>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now);
            if (inConstructionCrystalMine != null)
            {
                currentCrystalMine.EnableDate = inConstructionCrystalMine.EnableDate;
            }

            var currentMetalMine = repository.Max<MetalMine, int>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now, x => x.Level)
                ?? new MetalMine { MineType = MineType.Metal, Planet = new BlackPlanet(), Cost = new Cost() };
            var inConstructionMetalMine = repository.Get<MetalMine>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now);
            if (inConstructionMetalMine != null)
            {
                currentMetalMine.EnableDate = inConstructionMetalMine.EnableDate;
            }

            var currentDarkMatterMine = repository.Max<DarkMatterMine, int>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now, x => x.Level)
                ?? new DarkMatterMine { MineType = MineType.DarkMatter, Planet = new BlackPlanet(), Cost = new Cost() };
            var inConstructionDarkMatterMine = repository.Get<DarkMatterMine>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now);
            if (inConstructionDarkMatterMine != null)
            {
                currentDarkMatterMine.EnableDate = inConstructionDarkMatterMine.EnableDate;
            }
            
            return new List<Mine>{currentCrystalMine, currentMetalMine, currentDarkMatterMine };
        }

        
        public IList<Mine> GetMinesToBuild(int planetId)
        {
            var currentCrystalMineLevel = repository.Max<CrystalMine, int>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now, x => x.Level) != null
                ? repository.Max<CrystalMine, int>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now, x => x.Level).Level : 0;
            var currentMetalMineLevel = repository.Max<MetalMine, int>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now, x => x.Level) != null
                ? repository.Max<MetalMine, int>(x => x.Planet.Id == planetId, x => x.Level).Level : 0;
            var currentDarkMatterMineLevel = repository.Max<DarkMatterMine, int>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now, x => x.Level) != null
                ? repository.Max<DarkMatterMine, int>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now, x => x.Level).Level : 0;

            var mines = new List<Mine>();
            
            var newCrystalMine = new CrystalMine {
                Cost = repository.Get<Cost>(x => x.Element == "crystal mine L" + (currentCrystalMineLevel + 1).ToString()),
                EnergyConsumption = 50 * (currentCrystalMineLevel + 1),
                ConstructionTime = 1 + (20 * currentCrystalMineLevel),
                Productivity = 50 * (currentCrystalMineLevel + 1),
                MineType = MineType.Crystal,
                Level = currentCrystalMineLevel + 1
            };
            mines.Add(newCrystalMine);

            var newMetalMine = new MetalMine {
                Cost = repository.Get<Cost>(x => x.Element == "metal mine L" + (currentCrystalMineLevel + 1).ToString()),
                EnergyConsumption = 50 + (25 * currentMetalMineLevel),
                ConstructionTime = 1 + (10 * currentMetalMineLevel),
                Productivity = 100 * (currentMetalMineLevel + 1),
                MineType = MineType.Metal,
                Level = currentMetalMineLevel + 1
            };
            mines.Add(newMetalMine);

            var newDarkMatterMine = new DarkMatterMine
            {
                Cost = repository.Get<Cost>(x => x.Element == "dark matter mine L" + (currentCrystalMineLevel + 1).ToString()),
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
            var result = repository.Add(mine);
            repository.SaveChanges();
            return result;
        }

        public Mine GetFromDTO(MineDTO dto)
        {
            Mine mine = null;
            switch (dto.MineType)
            {
                case MineType.Crystal:
                    mine = new CrystalMine
                    {
                        Id = dto.Id,
                        ConstructionTime = dto.ConstructionTime,
                        Cost = repository.Get<Cost>(dto.CostId),
                        EnergyConsumption = dto.EnergyConsumption,
                        Level = dto.Level,
                        MineType = MineType.Crystal,
                        Planet = repository.Get<Planet>(dto.PlanetId),
                        Productivity = dto.Productivity
                    };
                    break;
                case MineType.Metal:
                    mine = new MetalMine
                    {
                        Id = dto.Id,
                        ConstructionTime = dto.ConstructionTime,
                        Cost = repository.Get<Cost>(dto.CostId),
                        EnergyConsumption = dto.EnergyConsumption,
                        Level = dto.Level,
                        MineType = MineType.Metal,
                        Planet = repository.Get<Planet>(dto.PlanetId),
                        Productivity = dto.Productivity
                    };
                    break;
                case MineType.DarkMatter:
                    mine = new DarkMatterMine
                    {
                        Id = dto.Id,
                        ConstructionTime = dto.ConstructionTime,
                        Cost = repository.Get<Cost>(dto.CostId),
                        EnergyConsumption = dto.EnergyConsumption,
                        Level = dto.Level,
                        MineType = MineType.DarkMatter,
                        Planet = repository.Get<Planet>(dto.PlanetId),
                        Productivity = dto.Productivity
                    };
                    break;
            }
            return mine;
        }

        public void FinishMine(Timer timer, MineDTO mineDto)
        {
            //destruyo el timer
            timer.Stop();
            timer.Dispose();

            //mandar notificacion push al usuario
        }
    }
}
