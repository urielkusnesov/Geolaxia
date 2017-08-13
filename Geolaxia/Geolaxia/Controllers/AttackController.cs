using Geolaxia.Models;
using log4net;
using Model;
using Model.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Attacks;
using Service.Planets;
using Service.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Geolaxia.Controllers
{
    public class AttackController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PlanetController));
        private IPlayerService playerService;
        private IPlanetService planetService;
        private IAttackService service;

        public AttackController(IAttackService service, IPlayerService playerService, IPlanetService planetService) : base(playerService)
        {
            this.service = service;
            this.planetService = planetService;
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

        // POST api/attack/galaxies
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
            catch (Exception e)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        // POST api/attack/solarsystems
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
                IList<SolarSystem> solarSystems = planetService.GetSolarSystemsByGalaxy(galaxyId);
                var okResponse = new ApiResponse { Data = solarSystems, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                return json;
            }
            catch (Exception e)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }

        // POST api/attack/planetbyss
        public JObject SolarSystems(int solarSystemId)
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
            catch (Exception e)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
                return json;
            }
        }
    }
}
