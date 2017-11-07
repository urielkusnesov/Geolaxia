using Geolaxia.Models;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Construction;
using Service.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.Web.Http;
using Model;
using Model.DTO;
using Service.Planets;

namespace Geolaxia.Controllers
{
    public class ConstructionController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ConstructionController));
        private IConstructionService service;
        private IPlayerService playerService;
        private IPlanetService planetService;

        private static System.Timers.Timer aTimer;

        public ConstructionController(IConstructionService service, IPlayerService playerService, IPlanetService planetService)
            :base(playerService)
        {
            this.service = service;
            this.playerService = playerService;
            this.planetService = planetService;
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
                SetEnergyFacilityTimer(new EnergyFacilityDTO{ConstructionTime = 2});

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
                SetEnergyFacilityTimer(new EnergyFacilityDTO { ConstructionTime = 2 });

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
    }
}
