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
    }
}
