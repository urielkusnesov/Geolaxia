using Geolaxia.Models;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Construction;
using Service.Players;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Web.Http;
using Model;
using Model.DTO;
using Model.Enum;
using Service.Defenses;
using Service.Planets;
using Service.Ships;

namespace Geolaxia.Controllers
{
    public class ConstructionController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ConstructionController));
        private IConstructionService service;
        private IPlayerService playerService;
        private IPlanetService planetService;
        private IShipService shipService;
        private IDefenseService defenseService;

        private static System.Timers.Timer aTimer;

        public ConstructionController(IConstructionService service, IPlayerService playerService, IPlanetService planetService, IShipService shipService, IDefenseService defenseService)
            :base(playerService)
        {
            this.service = service;
            this.playerService = playerService;
            this.planetService = planetService;
            this.shipService = shipService;
            this.defenseService = defenseService;
        }

        //api/construction/currentmines
        [HttpPost]
        public JObject CurrentMines(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting current mines from planet: " + planetId);
            try
            {
                var mines = service.GetCurrentMines(planetId);
                var okResponse = new ApiResponse { Data = mines, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/construction/minestobuild
        [HttpPost]
        public JObject MinesToBuild(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting mines to build from planet: " + planetId);
            try
            {
                var mines = service.GetMinesToBuild(planetId);
                var okResponse = new ApiResponse { Data = mines, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/construction/startbuildmine
        [HttpPost]
        public JObject StartBuildMine(MineDTO mineDto)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            var mine = service.GetFromDTO(mineDto);
            mine.EnableDate = DateTime.Now.AddMinutes(mine.ConstructionTime);
            var newMine = service.Add(mine);
            if (newMine != null)
            {
                planetService.UseResources(mine.Planet.Id, mine.Cost);
                logger.Info("start to build mine in planet: " + mineDto.PlanetId);
                SetMineTimer(mineDto);

                var okResponse = new ApiResponse { Data = newMine, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            else
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "No se pudo realizar la construccion. Intente nuevamente" } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/construction/currentenergyfacilities
        [HttpPost]
        public JObject CurrentEnergyFacilities(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting current energy facilities from planet: " + planetId);
            try
            {
                var energyCentral = service.GetCurrentEnergyCentral(planetId);
                var energyFuelCentral = service.GetCurrentEnergyFuelCentral(planetId);
                var solarPanels = service.GetCurrentSolarPanels(planetId);
                var windTurbines = service.GetCurrentWindTurbines(planetId);
                IList<IList<EnergyFacility>> energyFacilities = new List<IList<EnergyFacility>>
                {
                    new List<EnergyFacility>{energyCentral},
                    new List<EnergyFacility>{energyFuelCentral},
                    new List<EnergyFacility>(solarPanels),
                    new List<EnergyFacility>(windTurbines)
                };
                var okResponse = new ApiResponse { Data = energyFacilities, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/construction/energyfacilitiestobuild
        [HttpPost]
        public JObject EnergyFacilitiesToBuild(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting mines to build from planet: " + planetId);
            try
            {
                var energyFacilities = service.GetEnergyFacilitiesToBuild(planetId);
                var okResponse = new ApiResponse { Data = energyFacilities, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/construction/startbuildenergyfacility
        [HttpPost]
        public JObject StartBuildEnergyFacility(EnergyFacilityDTO energyFacilityDto)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            var energyFacility = service.GetFromDTO(energyFacilityDto);
            energyFacility.EnableDate = DateTime.Now.AddMinutes(energyFacility.ConstructionTime);
            var newEnergyFacility = service.Add(energyFacility);
            if (newEnergyFacility != null)
            {
                planetService.UseResources(energyFacility.Planet.Id, energyFacility.Cost);
                logger.Info("start to build energy facility in planet: " + energyFacilityDto.PlanetId);
                SetEnergyFacilityTimer(energyFacilityDto);

                var okResponse = new ApiResponse { Data = newEnergyFacility, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            else
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "No se pudo realizar la construccion. Intente nuevamente" } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/construction/startbuildsolarpanel
        [HttpPost]
        public JObject StartBuildSolarPanel(int planetId, int qtt)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            var solarPanel = new SolarPanel {Cost = new Cost(), Planet = planetService.Get(planetId), ConstructionTime = 2};
            solarPanel.EnableDate = DateTime.Now.AddMinutes(solarPanel.ConstructionTime * qtt);
            try
            {
                service.AddSolarPanels(planetId, qtt);
                planetService.UseResources(planetId, new Cost{CrystalCost = 5, MetalCost = 20});
                logger.Info("start to build solar panels in planet: " + planetId);
                SetEnergyFacilityTimer(new EnergyFacilityDTO{ConstructionTime = 2, PlanetId = planetId, EnergyFacilityType = EnergyFacilityType.SolarPanel});

                var okResponse = new ApiResponse { Data = solarPanel, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "No se pudo realizar la construccion. Intente nuevamente" } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/construction/startbuildwindturbine
        [HttpPost]
        public JObject StartBuildWindTurbine(int planetId, int qtt)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            var windTurbine = new WindTurbine { Cost = new Cost(), Planet = planetService.Get(planetId), ConstructionTime = 2};
            windTurbine.EnableDate = DateTime.Now.AddMinutes(windTurbine.ConstructionTime * qtt);
            try
            {
                service.AddWindTurbines(planetId, qtt);
                planetService.UseResources(planetId, new Cost { CrystalCost = 5, MetalCost = 20 });
                logger.Info("start to build wind turbines in planet: " + planetId);
                SetEnergyFacilityTimer(new EnergyFacilityDTO { ConstructionTime = 2, PlanetId = planetId, EnergyFacilityType = EnergyFacilityType.WindTurbine});

                var okResponse = new ApiResponse { Data = windTurbine, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "No se pudo realizar la construccion. Intente nuevamente" } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        private void SetMineTimer(MineDTO mineDto)
        {
            var msUntilFinish = mineDto.ConstructionTime * 60 * 1000;

            // Create a timer with a timeToArrival interval.
            aTimer = new Timer(msUntilFinish);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += (sender, e) => service.FinishMine(aTimer, mineDto);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            aTimer.Start();
        }

        private void SetEnergyFacilityTimer(EnergyFacilityDTO energyFacilityDto)
        {
            var msUntilFinish = energyFacilityDto.ConstructionTime * 60 * 1000;

            // Create a timer with a timeToArrival interval.
            aTimer = new Timer(msUntilFinish);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += (sender, e) => service.FinishEnergyFacility(aTimer, energyFacilityDto);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            aTimer.Start();
        }

        //api/construction/currenthangar
        [HttpPost]
        public JObject CurrentHangar(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting current hangar from planet: " + planetId);
            try
            {
                var hangar = service.GetCurrentHangar(planetId);
                var okResponse = new ApiResponse { Data = hangar, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/construction/startbuildhangar
        [HttpPost]
        public JObject StartBuildHangar(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            var newHangar = service.AddHangar(planetId);
            if (newHangar != null)
            {
                planetService.UseResources(newHangar.Planet.Id, newHangar.Cost);
                logger.Info("start to build hangar in planet: " + newHangar.Planet.Id);
                SetHangarTimer(newHangar);

                var okResponse = new ApiResponse { Data = newHangar, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            else
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "No se pudo realizar la construccion. Intente nuevamente" } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        private void SetHangarTimer(Hangar hangar)
        {
            var msUntilFinish = hangar.ConstructionTime * 60 * 1000;

            // Create a timer with a timeToArrival interval.
            aTimer = new Timer(msUntilFinish);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += (sender, e) => service.FinishHangar(aTimer, hangar);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            aTimer.Start();
        }

        //api/construction/currentships
        [HttpPost]
        public JObject CurrentShips(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting current ships from planet: " + planetId);
            try
            {
                var ships = service.GetCurrentShips(planetId);
                var okResponse = new ApiResponse { Data = ships, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/construction/shipscost
        [HttpGet]
        public JObject ShipsCost()
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting ships cost");
            try
            {
                var ships = shipService.GetShipsCost();
                var okResponse = new ApiResponse { Data = ships, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/construction/startbuildship
        [HttpPost]
        public JObject StartBuildShip(int planetId, int qtt, ShipType shipType)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            Ship ship;
            switch (shipType)
            {
                case ShipType.X:
                    ship = new ShipX { Cost = new Cost(), Planet = planetService.Get(planetId), ConstructionTime = 5 };
                    break;
                case ShipType.Y:
                    ship = new ShipY { Cost = new Cost(), Planet = planetService.Get(planetId), ConstructionTime = 10 };
                    break;
                case ShipType.Z:
                    ship = new ShipZ { Cost = new Cost(), Planet = planetService.Get(planetId), ConstructionTime = 15 };
                    break;
                default:
                    ship = new ShipX { Cost = new Cost(), Planet = planetService.Get(planetId), ConstructionTime = 5 };
                    break;
            }
            ship.EnableDate = DateTime.Now.AddMinutes(ship.ConstructionTime*qtt);
            try
            {
                service.AddShips(planetId, qtt, shipType);
                planetService.UseResources(planetId, new Cost { CrystalCost = 5, MetalCost = 20 });
                logger.Info("start to build ships in planet: " + planetId);
                SetShipTimer(ship);

                var okResponse = new ApiResponse { Data = ship, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch
            {
                var response = new ApiResponse {Status = new Status {Result = "error", Description = "No se pudo realizar la construccion. Intente nuevamente"}};
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        private void SetShipTimer(Ship ship)
        {
            var msUntilFinish = ship.ConstructionTime * 60 * 1000;

            // Create a timer with a timeToArrival interval.
            aTimer = new Timer(msUntilFinish);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += (sender, e) => shipService.FinishShip(aTimer, ship);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            aTimer.Start();
        }

        //api/construction/currentshield
        [HttpPost]
        public JObject CurrentShield(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting current shield from planet: " + planetId);
            try
            {
                var shield = defenseService.GetCurrentShield(planetId);
                var okResponse = new ApiResponse { Data = shield, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/construction/currentprobes
        [HttpPost]
        public JObject CurrentProbes(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting current probes from planet: " + planetId);
            try
            {
                var probes = service.GetCurrentProbes(planetId);
                var okResponse = new ApiResponse { Data = probes, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/construction/currenttraders
        [HttpPost]
        public JObject CurrentTraders(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting current traders from planet: " + planetId);
            try
            {
                var traders = service.GetCurrentTraders(planetId);
                var okResponse = new ApiResponse { Data = traders, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/construction/startbuildshield
        [HttpPost]
        public JObject StartBuildShield(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            var newShield = service.AddShield(planetId);
            if (newShield != null)
            {
                planetService.UseResources(newShield.Planet.Id, newShield.Cost);
                logger.Info("start to build shield in planet: " + newShield.Planet.Id);
                SetShieldTimer(newShield);

                var okResponse = new ApiResponse { Data = newShield, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            else
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "No se pudo realizar la construccion. Intente nuevamente" } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/construction/startbuildprobe
        [HttpPost]
        public JObject StartBuildProbe(int planetId, int qtt)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            var probe = new Probe { Cost = new Cost(), Planet = planetService.Get(planetId), ConstructionTime = 30 };
            probe.EnableDate = DateTime.Now.AddMinutes(probe.ConstructionTime * qtt);
            try
            {
                service.AddProbes(planetId, qtt);
                planetService.UseResources(planetId, new Cost { CrystalCost = 3000, MetalCost = 3000 });
                logger.Info("start to build probes in planet: " + planetId);
                SetProbeTimer(new Probe { ConstructionTime = 30}, planetId);

                var okResponse = new ApiResponse { Data = probe, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "No se pudo realizar la construccion. Intente nuevamente" } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/construction/startbuildtrader
        [HttpPost]
        public JObject StartBuildTrader(int planetId, int qtt)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            var trader = new Trader { Cost = new Cost(), Planet = planetService.Get(planetId), ConstructionTime = 30 };
            trader.EnableDate = DateTime.Now.AddMinutes(trader.ConstructionTime * qtt);
            try
            {
                service.AddProbes(planetId, qtt);
                planetService.UseResources(planetId, new Cost { CrystalCost = 3000, MetalCost = 3000 });
                logger.Info("start to build traders in planet: " + planetId);
                SetTraderTimer(new Trader { ConstructionTime = 30 }, planetId);

                var okResponse = new ApiResponse { Data = trader, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "No se pudo realizar la construccion. Intente nuevamente" } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        private void SetShieldTimer(Shield shield)
        {
            var msUntilFinish = shield.ConstructionTime * 60 * 1000;

            // Create a timer with a timeToArrival interval.
            aTimer = new Timer(msUntilFinish);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += (sender, e) => service.FinishShield(aTimer, shield);
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
            aTimer.Start();
        }

        private void SetProbeTimer(Probe probe, int planetId)
        {
            var msUntilFinish = probe.ConstructionTime * 60 * 1000;

            // Create a timer with a timeToArrival interval.
            aTimer = new Timer(msUntilFinish);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += (sender, e) => service.FinishProbe(aTimer, probe, planetId);
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
            aTimer.Start();
        }

        private void SetTraderTimer(Trader trader, int planetId)
        {
            var msUntilFinish = trader.ConstructionTime * 60 * 1000;

            // Create a timer with a timeToArrival interval.
            aTimer = new Timer(msUntilFinish);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += (sender, e) => service.FinishTrader(aTimer, trader, planetId);
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
            aTimer.Start();
        }
    }
}
