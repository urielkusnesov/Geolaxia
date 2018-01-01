using System;
using Model;
using Repository;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Model.DTO;
using Model.Enum;
using Service.Notification;
using Service.Planets;
using Service.Players;

namespace Service.Construction
{
    public class ConstructionService : IConstructionService
    {
        private readonly IRepositoryService repository;
        private IPlanetService planetService;
        private INotificationService notificationService;

        public ConstructionService(IRepositoryService repository, IPlanetService planetService, INotificationService notificationService)
        {
            this.repository = repository;
            this.planetService = planetService;
            this.notificationService = notificationService;
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

        public IList<long> GetMinesBuildingTime(int planetId)
        {
            var mineBuildingTimes = new List<long>();

            var inConstructionCrystalMine = repository.Get<CrystalMine>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now);
            mineBuildingTimes.Add(inConstructionCrystalMine != null ? GetMilli(inConstructionCrystalMine.EnableDate) : GetMilli(DateTime.MinValue));

            var inConstructionMetalMine = repository.Get <MetalMine>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now);
            mineBuildingTimes.Add(inConstructionMetalMine != null ? GetMilli(inConstructionMetalMine.EnableDate) : GetMilli(DateTime.MinValue));

            var inConstructionDarkMatterMine = repository.Get<DarkMatterMine>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now);
            mineBuildingTimes.Add(inConstructionDarkMatterMine != null ? GetMilli(inConstructionDarkMatterMine.EnableDate) : GetMilli(DateTime.MinValue));

            return mineBuildingTimes;
        }

        private long GetMilli(DateTime date)
        {
            return Convert.ToInt64((date - DateTime.Now).TotalMilliseconds);
            //return (Convert.ToInt64(date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds));
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
            using (var context = new GeolaxiaContext())
            {
                try
                {
                    var repo = new RepositoryService(context);
                    var planet = repo.Get<Planet>(x => x.Id == mineDto.PlanetId);
                    var player = repo.Get<Player>(x => x.Id == planet.Conqueror.Id);
                    notificationService.SendPushNotification(player.FirebaseToken, "Construccion finalizada", "Finalizo la construccion de la mina" + mineDto.MineType, "ConstructionsActivity");
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }

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

        public IList<long> GetEnergyFacilitiesBuildingTime(int planetId)
        {
            var energyFacilityBuildingTimes = new List<long>();

            var inConstructionEnergyCentral = repository.Get<EnergyCentral>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now);
            energyFacilityBuildingTimes.Add(inConstructionEnergyCentral != null ? GetMilli(inConstructionEnergyCentral.EnableDate) : GetMilli(DateTime.MinValue));

            var inConstructionEnergyFuelCentral = repository.Get<EnergyFuelCentral>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now);
            energyFacilityBuildingTimes.Add(inConstructionEnergyFuelCentral != null ? GetMilli(inConstructionEnergyFuelCentral.EnableDate) : GetMilli(DateTime.MinValue));

            var inConstructionSolarPanel = repository.Max<SolarPanel, DateTime>(x => x.Planet.Id.Equals(planetId) && x.EnableDate > DateTime.Now, x => x.EnableDate);
            energyFacilityBuildingTimes.Add(inConstructionSolarPanel != null ? GetMilli(inConstructionSolarPanel.EnableDate) : GetMilli(DateTime.MinValue));

            var inConstructionWindTurbine = repository.Max<WindTurbine, DateTime>(x => x.Planet.Id.Equals(planetId) && x.EnableDate > DateTime.Now, x => x.EnableDate);
            energyFacilityBuildingTimes.Add(inConstructionWindTurbine != null ? GetMilli(inConstructionWindTurbine.EnableDate) : GetMilli(DateTime.MinValue));

            return energyFacilityBuildingTimes;
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
            using (var context = new GeolaxiaContext())
            {
                try
                {
                    var repo = new RepositoryService(context);
                    var planet = repo.Get<Planet>(x => x.Id == energyFacilityDto.PlanetId);
                    var player = repo.Get<Player>(x => x.Id == planet.Conqueror.Id);
                    notificationService.SendPushNotification(player.FirebaseToken, "Construccion finalizada", "Finalizo la construccion de " + energyFacilityDto.EnergyFacilityType, "ConstructionsActivity");
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }

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
            for (var i = 1; i <= qtt; i++)
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
            for (var i = 1; i <= qtt; i++)
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

        public Hangar GetCurrentHangar(int planetId)
        {
            var hangar = repository.Get<Hangar>(x => x.Planet.Id == planetId)
                ?? new Hangar { Planet = new BlackPlanet(), Cost = new Cost{CrystalCost = 2000, MetalCost = 2000, DarkMatterCost = 500}, RequiredLevel = 3};

            return hangar;
        }

        public IList<long> GetHangarBuildingTime(int planetId)
        {
            var inConstructionHangar = repository.Get<Hangar>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now);
            return inConstructionHangar != null ? new List<long>{GetMilli(inConstructionHangar.EnableDate)} : new List<long>{GetMilli(DateTime.MinValue)};
        }

        public Hangar AddHangar(int planetId)
        {
            var currentHangar = repository.Get<Hangar>(x => x.Planet.Id == planetId);
            if (currentHangar != null)
            {
                return currentHangar;
            }

            var cost = repository.Get<Cost>(x => x.Element == "hangar");
            var planet = repository.Get<Planet>(planetId);

            var hangar = new Hangar
            {
                Planet = planet,
                Cost = cost,
                EnableDate = DateTime.Now.AddMinutes(60),
                ConstructionTime = 60,
                RequiredLevel = 3,
                Name = "Hangar " + planet.Name
            };

            var result = repository.Add(hangar);
            repository.SaveChanges();
            return result;                
        }

        public void FinishHangar(Timer timer, Hangar hangar)
        {
            //mandar notificacion push al usuario
            using (var context = new GeolaxiaContext())
            {
                try
                {
                    var repo = new RepositoryService(context);
                    var planet = repo.Get<Planet>(x => x.Id == hangar.Planet.Id);
                    var player = repo.Get<Player>(x => x.Id == planet.Conqueror.Id);
                    notificationService.SendPushNotification(player.FirebaseToken, "Construccion finalizada", "Finalizo la construccion del hangar", "ConstructionsActivity");
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }

            //elimino el timer
            timer.Stop();
            timer.Dispose();
        }

        public IList<Ship> GetCurrentShips(int planetId)
        {
            return repository.List<Ship>(x => x.Planet.Id == planetId);
        }

        public void AddShips(int planetId, int qtt, ShipType shipType)
        {
            switch (shipType)
            {
                case ShipType.X:
                    AddShipX(planetId, qtt);
                    break;
                case ShipType.Y:
                    AddShipY(planetId, qtt);
                    break;
                case ShipType.Z:
                    AddShipZ(planetId, qtt);
                    break;
            }
        }

        public IList<long> GetShipsBuildingTime(int planetId)
        {
            var shipsBuildingTimes = new List<long>();

            var inConstructionShipX = repository.Max<ShipX, DateTime>(x => x.Planet.Id.Equals(planetId) && x.EnableDate > DateTime.Now, x => x.EnableDate);
            shipsBuildingTimes.Add(inConstructionShipX != null ? GetMilli(inConstructionShipX.EnableDate) : GetMilli(DateTime.MinValue));

            var inConstructionShipY = repository.Max<ShipY, DateTime>(x => x.Planet.Id.Equals(planetId) && x.EnableDate > DateTime.Now, x => x.EnableDate);
            shipsBuildingTimes.Add(inConstructionShipY != null ? GetMilli(inConstructionShipY.EnableDate) : GetMilli(DateTime.MinValue));

            var inConstructionShipZ = repository.Max<ShipZ, DateTime>(x => x.Planet.Id.Equals(planetId) && x.EnableDate > DateTime.Now, x => x.EnableDate);
            shipsBuildingTimes.Add(inConstructionShipZ != null ? GetMilli(inConstructionShipZ.EnableDate) : GetMilli(DateTime.MinValue));

            return shipsBuildingTimes;
        }

        public void AddShipX(int planetId, int qtt)
        {
            var planet = repository.Get<Planet>(planetId);
            var cost = repository.Get<Cost>(x => x.Element == "ship x");
            var currentShips = repository.Count<ShipX>(x => x.Planet.Id == planetId);
            for (var i = 1; i <= qtt; i++)
            {
                repository.Add(new ShipX
                {
                    ShipType = ShipType.X,
                    Planet = planet,
                    Cost = cost,
                    ConstructionTime = 5,
                    EnableDate = DateTime.Now.AddMinutes(i * 5),
                    Attack = 100,
                    Defence = 100,
                    Speed = 10,
                    DarkMatterConsumption = 10,
                    RequiredLevel = 2,
                    Name = "Ship X" + currentShips + 1 + " " + planet.Name
                });
                currentShips++;
            }

            repository.SaveChanges();
        }

        public void AddShipY(int planetId, int qtt)
        {
            var planet = repository.Get<Planet>(planetId);
            var cost = repository.Get<Cost>(x => x.Element == "ship y");
            var currentShips = repository.Count<ShipY>(x => x.Planet.Id == planetId);
            for (var i = 1; i <= qtt; i++)
            {
                repository.Add(new ShipY
                {
                    ShipType = ShipType.Y,
                    Planet = planet,
                    Cost = cost,
                    ConstructionTime = 10,
                    EnableDate = DateTime.Now.AddMinutes(i * 10),
                    Attack = 150,
                    Defence = 100,
                    Speed = 15,
                    DarkMatterConsumption = 15,
                    RequiredLevel = 3,
                    Name = "Ship Y" + currentShips + 1 + " " + planet.Name
                });
                currentShips++;
            }

            repository.SaveChanges();
        }

        public void AddShipZ(int planetId, int qtt)
        {
            var planet = repository.Get<Planet>(planetId);
            var cost = repository.Get<Cost>(x => x.Element == "ship z");
            var currentShips = repository.Count<ShipZ>(x => x.Planet.Id == planetId);
            for (var i = 1; i <= qtt; i++)
            {
                repository.Add(new ShipZ
                {
                    ShipType = ShipType.Z,
                    Planet = planet,
                    Cost = cost,
                    ConstructionTime = 15,
                    EnableDate = DateTime.Now.AddMinutes(i * 15),
                    Attack = 300,
                    Defence = 200,
                    Speed = 20,
                    DarkMatterConsumption = 30,
                    RequiredLevel = 5,
                    Name = "Ship Z" + currentShips + 1 + " " + planet.Name
                });
                currentShips++;
            }

            repository.SaveChanges();
        }

        public IList<Probe> GetCurrentProbes(int planetId)
        {
            var probes = repository.List<Probe>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now).ToList();
            var inConstructionProbe = repository.Max<Probe, DateTime>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now, x => x.EnableDate);
            if (inConstructionProbe != null)
            {
                if (probes.Any())
                {
                    probes.First().EnableDate = inConstructionProbe.EnableDate;
                }
                else
                {
                    probes.Add(new Probe
                    {
                        EnableDate = inConstructionProbe.EnableDate,
                        Planet = new BlackPlanet(),
                        Cost = new Cost(),
                        RequiredLevel = 5
                    });
                }
            }

            return probes;
        }

        public IList<Trader> GetCurrentTraders(int planetId)
        {
            var traders = repository.List<Trader>(x => x.Planet.Id == planetId && x.EnableDate <= DateTime.Now).ToList();
            var inConstructionTrader = repository.Max<Trader, DateTime>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now, x => x.EnableDate);
            if (inConstructionTrader != null)
            {
                if (traders.Any())
                {
                    traders.First().EnableDate = inConstructionTrader.EnableDate;
                }
                else
                {
                    traders.Add(new Trader
                    {
                        EnableDate = inConstructionTrader.EnableDate,
                        Planet = new BlackPlanet(),
                        Cost = new Cost(),
                        RequiredLevel = 5
                    });
                }
            }

            return traders;
        }

        public IList<long> GetOthersBuildingTime(int planetId)
        {
            var otherBuildingTimes = new List<long>();

            var inConstructionShield = repository.Get<Shield>(x => x.Planet.Id == planetId && x.EnableDate > DateTime.Now);
            otherBuildingTimes.Add(inConstructionShield != null ? GetMilli(inConstructionShield.EnableDate) : GetMilli(DateTime.MinValue));

            var inConstructionProbe = repository.Max<Probe, DateTime>(x => x.Planet.Id.Equals(planetId) && x.EnableDate > DateTime.Now, x => x.EnableDate);
            otherBuildingTimes.Add(inConstructionProbe != null ? GetMilli(inConstructionProbe.EnableDate) : GetMilli(DateTime.MinValue));

            var inConstructionTrader = repository.Max<Trader, DateTime>(x => x.Planet.Id.Equals(planetId) && x.EnableDate > DateTime.Now, x => x.EnableDate);
            otherBuildingTimes.Add(inConstructionTrader != null ? GetMilli(inConstructionTrader.EnableDate) : GetMilli(DateTime.MinValue));

            return otherBuildingTimes;
        }

        public Shield AddShield(int planetId)
        {
            var currentShield = repository.Get<Shield>(x => x.Planet.Id == planetId);
            if (currentShield != null)
            {
                return currentShield;
            }

            var cost = repository.Get<Cost>(x => x.Element == "shield");
            var planet = repository.Get<Planet>(planetId);

            var shield = new Shield
            {
                Planet = planet,
                Cost = cost,
                EnableDate = DateTime.Now.AddMinutes(120),
                ConstructionTime = 120,
                RequiredLevel = 5,
                Name = "Shield " + planet.Name
            };

            var result = repository.Add(shield);
            repository.SaveChanges();
            return result;
        }

        public void AddProbes(int planetId, int qtt)
        {
            var planet = repository.Get<Planet>(planetId);
            var cost = repository.Get<Cost>(x => x.Element == "probe");
            for (int i = 0; i < qtt; i++)
            {
                repository.Add(new Probe
                {
                    Planet = planet,
                    Cost = cost,
                    ConstructionTime = 2,
                    EnableDate = DateTime.Now.AddMinutes(i * 30),
                    ShipType = ShipType.Probe,
                    RequiredLevel = 5,
                    DarkMatterConsumption = 10,
                    Speed = 50
                });
            }

            repository.SaveChanges();
        }

        public void AddTraders(int planetId, int qtt)
        {
            var planet = repository.Get<Planet>(planetId);
            var cost = repository.Get<Cost>(x => x.Element == "solar panel");
            for (int i = 0; i < qtt; i++)
            {
                repository.Add(new Trader
                {
                    Planet = planet,
                    Cost = cost,
                    ConstructionTime = 2,
                    EnableDate = DateTime.Now.AddMinutes(i * 2),
                    ShipType = ShipType.Trader,
                    RequiredLevel = 5,
                    DarkMatterConsumption = 30,
                    Speed = 50,
                    MaxCrystal = 1000,
                    MaxMetal = 1000,
                    MaxDarkMatter = 500
                });
            }

            repository.SaveChanges();
        }

        public void FinishShield(Timer timer, Shield shield)
        {
            //mandar notificacion push al usuario
            using (var context = new GeolaxiaContext())
            {
                try
                {
                    var repo = new RepositoryService(context);
                    var planet = repo.Get<Planet>(x => x.Id == shield.Planet.Id);
                    var player = repo.Get<Player>(x => x.Id == planet.Conqueror.Id);
                    notificationService.SendPushNotification(player.FirebaseToken, "Construccion finalizada", "Finalizo la construccion del escudo", "ConstructionsActivity");
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }

            //elimino el timer
            timer.Stop();
            timer.Dispose();
        }

        public void FinishProbe(Timer timer, Probe probe, int planetId)
        {
            //mandar notificacion push al usuario
            using (var context = new GeolaxiaContext())
            {
                try
                {
                    var repo = new RepositoryService(context);
                    var planet = repo.Get<Planet>(x => x.Id == planetId);
                    var player = repo.Get<Player>(x => x.Id == planet.Conqueror.Id);
                    notificationService.SendPushNotification(player.FirebaseToken, "Construccion finalizada", "Finalizo la construccion de la sonda", "ConstructionsActivity");
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }

            //elimino el timer
            timer.Stop();
            timer.Dispose();
        }

        public void FinishTrader(Timer timer, Trader trader, int planetId)
        {
            //mandar notificacion push al usuario
            using (var context = new GeolaxiaContext())
            {
                try
                {
                    var repo = new RepositoryService(context);
                    var planet = repo.Get<Planet>(x => x.Id == planetId);
                    var player = repo.Get<Player>(x => x.Id == planet.Conqueror.Id);
                    notificationService.SendPushNotification(player.FirebaseToken, "Construccion finalizada", "Finalizo la construccion del carguero", "ConstructionsActivity");
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }

            //elimino el timer
            timer.Stop();
            timer.Dispose();
        }
    }
}
