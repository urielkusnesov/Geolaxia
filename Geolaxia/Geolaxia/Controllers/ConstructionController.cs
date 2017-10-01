using Geolaxia.Models;
using log4net;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Construction;
using Service.Players;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Geolaxia.Controllers
{
    public class ConstructionController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ConstructionController));
        private IConstructionService service;
        private IPlayerService playerService;

        public ConstructionController(IConstructionService service, IPlayerService playerService)
            :base(playerService)
        {
            this.service = service;
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

        //api/construction/buildcrystal
        [HttpPost]
        public JObject BuildMine(Mine mine)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("building crystal mine in planet: " + mine.Planet.Id);
            try
            {
                var newMine = service.Add(mine);
                if (newMine != null)
                {
                    var okResponse = new ApiResponse { Status = new Status { Result = "ok", Description = "" } };
                    var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                    return json;                    
                }
                else
                {
                    var response = new ApiResponse { Status = new Status { Result = "error", Description = "no se pudo contruir la mina. Intente nuevamente" } };
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
    }
}
