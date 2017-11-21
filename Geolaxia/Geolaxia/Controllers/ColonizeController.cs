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
using Service.Colonization;
using Timer = System.Timers.Timer;

namespace Geolaxia.Controllers
{
    public class ColonizeController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ColonizeController));
        private IPlayerService playerService;
        private IPlanetService planetService;
        private IShipService shipService;
        private IAttackService service;
        private IColonizeService colonizeService;

        private static System.Timers.Timer aTimer;

        public ColonizeController(IAttackService service, IPlayerService playerService, IPlanetService planetService, IShipService shipService, IColonizeService colonizeService)
            : base(playerService)
        {
            this.service = service;
            this.planetService = planetService;
            this.shipService = shipService;
            this.playerService = playerService;
            this.colonizeService = colonizeService;
        }

        //api/colonize/GetColonizers
        [HttpPost]
        public JObject GetColonizers(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting colonizers in planet: " + planetId);

            try
            {
                IList<Probe> colonizers = colonizeService.GetColonizers(planetId);
                var okResponse = new ApiResponse { Data = colonizers, Status = new Status { Result = "ok", Description = "" } };
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

        //api/colonize/SendColonizer
        [HttpPost]
        public JObject SendColonizer(int planetId, int planetIdTarget, long time)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("send colonizer to planet: " + planetIdTarget);

            try
            {
                colonizeService.SendColonizer(planetId, planetIdTarget, time);
                this.SetTimer(planetId, planetIdTarget, time);

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

        //api/colonize/IsSendingColonizer
        [HttpPost]
        public JObject IsSendingColonizer(int planetId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("is sending colonizer to planet: " + planetId);

            try
            {
                long sendingTime = colonizeService.IsSendingColonizer(planetId);
                var okResponse = new ApiResponse { Data = sendingTime, Status = new Status { Result = "ok", Description = "" } };
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

        private void SetTimer(int planetId, int planetIdTarget, long time)
        {
            aTimer = new Timer(time);

            aTimer.Elapsed += (sender, e) => colonizeService.PerformColonize(aTimer, planetId, planetIdTarget);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            aTimer.Start();
        }
    }
}
