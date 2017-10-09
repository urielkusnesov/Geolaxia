using Geolaxia.Models;
using log4net;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Construction;
using Service.Players;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Web.Http;
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

        //api/construction/startbuild
        [HttpPost]
        public JObject StartBuild(MineDTO mineDto)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("start to build mine in planet: " + mineDto.PlanetId);
            SetTimer(mineDto);
        
            var okResponse = new ApiResponse { Status = new Status { Result = "ok", Description = "" } };
            var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
            return json;
        }
        
        private void SetTimer(MineDTO mineDto)
        {
            var msUntilArrival = mineDto.ConstructionTime * 60 * 1000;

            // Create a timer with a timeToArrival interval.
            aTimer = new Timer(msUntilArrival);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += (sender, e) => service.BuildMine(aTimer, mineDto);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            aTimer.Start();
        }
    }
}
