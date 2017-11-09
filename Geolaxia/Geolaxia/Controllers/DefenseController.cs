using Geolaxia.Models;
using log4net;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Attacks;
using Service.Planets;
using Service.Players;
using Service.Ships;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Service.Defenses;

namespace Geolaxia.Controllers
{
    public class DefenseController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DefenseController));
        private IPlayerService playerService;
        private IPlanetService planetService;
        private IShipService shipService;
        private IAttackService service;
        private IDefenseService defenseService;

        private static System.Timers.Timer aTimer;

        public DefenseController(IAttackService service, IPlayerService playerService, IPlanetService planetService, IShipService shipService, IDefenseService defenseService)
            : base(playerService)
        {
            this.service = service;
            this.planetService = planetService;
            this.shipService = shipService;
            this.playerService = playerService;
            this.defenseService = defenseService;
        }

        //api/defense/GetCannons
        [HttpPost]
        public JObject GetCannons(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting cannons in planet: " + planetId);

            try
            {
                IList<Canon> cannons = defenseService.GetCanons(planetId);
                var okResponse = new ApiResponse { Data = cannons, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                
                return (json);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));

                return (json);
            }
        }

        //api/defense/GetShieldStatus
        [HttpPost]
        public JObject GetShieldStatus(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting shield status in planet: " + planetId);

            try
            {
                Shield shield = defenseService.GetShieldStatus(planetId);
                var okResponse = new ApiResponse { Data = shield, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

                return (json);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));

                return (json);
            }
        }

        //api/defense/BuildCannons
        [HttpPost]
        public JObject BuildCannons(int planetId, int cant)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("builing cannons in planet: " + planetId);

            try
            {
                defenseService.BuildCannons(planetId, cant);
                var okResponse = new ApiResponse { Data = string.Empty, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

                return (json);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));

                return (json);
            }
        }

        //api/defense/IsBuildingCannons
        [HttpPost]
        public JObject IsBuildingCannons(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting building cannons status in planet: " + planetId);

            try
            {
                long buldingTime = defenseService.IsBuildingCannons(planetId);
                var okResponse = new ApiResponse { Data = buldingTime, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

                return (json);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));

                return (json);
            }
        }

        //api/defense/Get3RandomQuestions
        [HttpPost]
        public JObject Get3RandomQuestions()
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting questions");

            try
            {
                List<Queztion> questions = defenseService.Get3RandomQuestions();
                var okResponse = new ApiResponse { Data = questions, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

                return (json);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = ex.Message } };
                JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));

                return (json);
            }
        }
    }
}
