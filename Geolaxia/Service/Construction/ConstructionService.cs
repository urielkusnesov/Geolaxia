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
            //mandar notificacion push al usuario

            //arranco el timer por hora de produccion
            timer.Stop();   
            var msUntilFinish = 60 * 60 * 1000;
            timer.Interval = msUntilFinish;
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += (sender, e) => planetService.AddMineResources(mineDto);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();

        }

        public EnergyCentral GetCurrentEnergyCentral(int planetId)
        {
            var currentEnergyCentral = repository.Max<EnergyCentral, int>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now, x => x.Level)
                ?? new EnergyCentral { EnergyFacilityType = EnergyFacilityType.EnergyCentral, Planet = new BlackPlanet(), Cost = new Cost() };
            var inConstructionEnergyCentral = repository.Get<EnergyCentral>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now);
            if (inConstructionEnergyCentral != null)
            {
                currentEnergyCentral.EnableDate = inConstructionEnergyCentral.EnableDate;
            }

            return currentEnergyCentral;
        }

        public EnergyFuelCentral GetCurrentEnergyFuelCentral(int planetId)
        {
            var currentEnergyFuelCentral = repository.Max<EnergyFuelCentral, int>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now, x => x.Level)
                ?? new EnergyFuelCentral { EnergyFacilityType = EnergyFacilityType.EnergyFuelCentral, Planet = new BlackPlanet(), Cost = new Cost() };
            var inConstructionEnergyFuelCentral = repository.Get<EnergyFuelCentral>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now);
            if (inConstructionEnergyFuelCentral != null)
            {
                currentEnergyFuelCentral.EnableDate = inConstructionEnergyFuelCentral.EnableDate;
            }

            return currentEnergyFuelCentral;
        }

        public IList<SolarPanel> GetCurrentSolarPanels(int planetId)
        {
            var solarPanels = repository.List<SolarPanel>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now).ToList();
            var inConstructionSolarPanel = repository.Max<SolarPanel, DateTime>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now, x => x.EnableDate);
            if (inConstructionSolarPanel != null)
            {
                if (solarPanels.Any())
                {
                    solarPanels.First().EnableDate = inConstructionSolarPanel.EnableDate;
                }
                else
                {
                    solarPanels.Add(new SolarPanel
                    {
                        EnergyFacilityType = EnergyFacilityType.SolarPanel,
                        EnableDate = inConstructionSolarPanel.EnableDate,
                        Planet = new BlackPlanet(),
                        Cost = new Cost()
                    });
                }
            }

            return solarPanels;
        }

        public IList<WindTurbine> GetCurrentWindTurbines(int planetId)
        {
            var windTurbines = repository.List<WindTurbine>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now).ToList();
            var inConstructionWindTurbine = repository.Max<WindTurbine, DateTime>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now, x => x.EnableDate);
            if (inConstructionWindTurbine != null)
            {
                if (windTurbines.Any())
                {
                    windTurbines.First().EnableDate = inConstructionWindTurbine.EnableDate;
                }
                else
                {
                    windTurbines.Add(new WindTurbine
                    {
                        EnergyFacilityType = EnergyFacilityType.WindTurbine,
                        EnableDate = inConstructionWindTurbine.EnableDate,
                        Planet = new BlackPlanet(),
                        Cost = new Cost()
                    });
                }
            }

            return windTurbines;
        }

        public IList<EnergyFacility> GetEnergyFacilitiesToBuild(int planetId)
        {
            var currentEnergyCentralLevel = repository.Max<EnergyCentral, int>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now, x => x.Level) != null
                ? repository.Max<EnergyCentral, int>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now, x => x.Level).Level : 0;
            var currentEnergyFuelCentralLevel = repository.Max<EnergyFuelCentral, int>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now, x => x.Level) != null
                ? repository.Max<EnergyFuelCentral, int>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now, x => x.Level).Level : 0;

            var energyFacilities = new List<EnergyFacility>();

            var newEnergyCentral = new EnergyCentral
            {
                Cost = repository.Get<Cost>(x => x.Element == "energy central L" + (currentEnergyCentralLevel + 1).ToString()),
                ConstructionTime = (int)Math.Pow(2, currentEnergyCentralLevel),
                Productivity = 100 * (currentEnergyCentralLevel + 1),
                EnergyFacilityType = EnergyFacilityType.EnergyCentral,
                Level = currentEnergyCentralLevel + 1,
            };
            energyFacilities.Add(newEnergyCentral);

            var newEnergyFuelCentral = new EnergyFuelCentral
            {
                Cost = repository.Get<Cost>(x => x.Element == "fuel energy central L" + (currentEnergyFuelCentralLevel + 1).ToString()),
                ConstructionTime = (int)Math.Pow(2, currentEnergyFuelCentralLevel + 1),
                Productivity = 100 * (currentEnergyFuelCentralLevel + 1),
                EnergyFacilityType = EnergyFacilityType.EnergyFuelCentral,
                Level = currentEnergyFuelCentralLevel + 1,
                DarkMatterConsumption = 25 * currentEnergyFuelCentralLevel + 10 * (currentEnergyFuelCentralLevel == 0 ? 1 : 0)
            };
            energyFacilities.Add(newEnergyFuelCentral);

            return energyFacilities;
        }

        public EnergyFacility Add(EnergyFacility energyFacility)
        {
            var result = repository.Add(energyFacility);
            repository.SaveChanges();
            return result;
        }

        public EnergyFacility GetFromDTO(EnergyFacilityDTO dto)
        {
            EnergyFacility energyFacility = null;
            switch (dto.EnergyFacilityType)
            {
                case EnergyFacilityType.EnergyCentral:
                    energyFacility = new EnergyCentral
                    {
                        Id = dto.Id,
                        ConstructionTime = dto.ConstructionTime,
                        Cost = repository.Get<Cost>(dto.CostId),
                        Level = dto.Level,
                        EnergyFacilityType = EnergyFacilityType.EnergyCentral,
                        Planet = repository.Get<Planet>(dto.PlanetId),
                        Productivity = dto.Productivity
                    };
                    break;
                case EnergyFacilityType.EnergyFuelCentral:
                    energyFacility = new EnergyFuelCentral
                    {
                        Id = dto.Id,
                        ConstructionTime = dto.ConstructionTime,
                        Cost = repository.Get<Cost>(dto.CostId),
                        Level = dto.Level,
                        EnergyFacilityType = EnergyFacilityType.EnergyFuelCentral,
                        Planet = repository.Get<Planet>(dto.PlanetId),
                        Productivity = dto.Productivity,
                        DarkMatterConsumption = dto.DarkMatterConsumption
                    };
                    break;
            }
            return energyFacility;
        }

        public void FinishEnergyFacility(Timer timer, EnergyFacilityDTO energyFacilityDto)
        {
            //mandar notificacion push al usuario

            //arranco el timer por hora para la produccion
            var msUntilFinish = 60 * 60 * 1000;
            timer.Interval = msUntilFinish;
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += (sender, e) => planetService.AddEnergy(energyFacilityDto);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();

        }

        public void AddSolarPanels(int planetId, int qtt)
        {
            var planet = repository.Get<Planet>(planetId);
            var cost = repository.Get<Cost>(x => x.Element == "solar panel");
            for (int i = 0; i < qtt; i++)
            {
                repository.Add(new SolarPanel
                {
                    CloudyProd = 10,
                    SunnyProd = 25,
                    RainyProd = 5,
                    Planet = planet,
                    Cost = cost,
                    ConstructionTime = 2,
                    EnableDate = DateTime.Now.AddMinutes(i*2),
                    EnergyFacilityType = EnergyFacilityType.SolarPanel
                });
            }

            repository.SaveChanges();
        }

        public void AddWindTurbines(int planetId, int qtt)
        {
            var planet = repository.Get<Planet>(planetId);
            var cost = repository.Get<Cost>(x => x.Element == "solar panel");
            for (int i = 0; i < qtt; i++)
            {
                repository.Add(new WindTurbine
                {
                    Threshold = 15,
                    BestProd = 25,
                    WorstProd = 10,
                    Planet = planet,
                    Cost = cost,
                    ConstructionTime = 2,
                    EnableDate = DateTime.Now.AddMinutes(i * 2),
                    EnergyFacilityType = EnergyFacilityType.WindTurbine
                });
            }

            repository.SaveChanges();
        }
    }
}
