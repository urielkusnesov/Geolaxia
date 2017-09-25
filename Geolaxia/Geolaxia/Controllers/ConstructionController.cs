using Geolaxia.Models;
using log4net;
using Model;
using Newtonsoft.Json.Linq;
using Service.Construction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Geolaxia.Controllers
{
    public class ConstructionController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ConstructionController));
        private IConstructionService service;

        public ConstructionController(IConstructionService service) 
        {
            this.service = service;
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
                IList<Mine> ships = service.GetMinesToBuild(planetId);
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
