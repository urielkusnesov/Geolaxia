using Geolaxia.Models;
using log4net;
using Model;
using Model.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Attacks;
using Service.Planets;
using Service.Players;
using Service.Ships;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Geolaxia.Controllers
{
    public class AttackController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(AttackController));
        private IPlayerService playerService;
        private IPlanetService planetService;
        private IShipService shipService;
        private IAttackService service;

        public AttackController(IAttackService service, IPlayerService playerService, IPlanetService planetService, IShipService shipService) : base(playerService)
        {
            this.service = service;
            this.planetService = planetService;
            this.shipService = shipService;
        }

        // POST api/attack/attack (json attack in body)
        [HttpPost]
        public JObject Attack(AttackDTO attackDto)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("adding new attack");
            try
            {
                var attack = service.GetFromDTO(attackDto);
                var newAttack = service.Add(attack);
                if (newAttack != null)
                {
                    logger.Info("attack added succesfully");
                    var response = new ApiResponse { Status = new Status { Result = "ok", Description = "" } };
                    JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                    return json;
                }
                else
                {
                    var response = new ApiResponse { Status = new Status { Result = "error", Description = "No se pudo realizar el ataque. Intente nuevamente" } };
                    JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                    return json;
                }
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        //api/attack/galaxies
        [HttpGet]
        public JObject Galaxies()
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting galaxies");
            try
            {
                IList<Galaxy> galaxies = planetService.GetAllGalaxies();
                var okResponse = new ApiResponse { Data = galaxies, Status = new Status { Result = "ok", Description = "" } };
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

        //api/attack/solarsystems
        [HttpPost]
        public JObject SolarSystems(int galaxyId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting solar systems in galaxy: " + galaxyId);
            try
            {
                IList<SolarSystem> solarSystems = planetService.GetSolarSystemsByGalaxy(Convert.ToInt32(galaxyId));
                var okResponse = new ApiResponse { Data = solarSystems, Status = new Status { Result = "ok", Description = "" } };
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

        //api/attack/planetbyss
        [HttpPost]
        public JObject PlanetsBySS(int solarSystemId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting planets in solar systems: " + solarSystemId);
            try
            {
                IList<Planet> planets = planetService.GetBySolarSystem(solarSystemId);
                var okResponse = new ApiResponse { Data = planets, Status = new Status { Result = "ok", Description = "" } };
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

        //api/attack/fleet
        [HttpPost]
        public JObject Fleet(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting fleet from planet: " + planetId);
            try
            {
                IList<Ship> ships = shipService.GetByPlanet(planetId);
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
    }
}
