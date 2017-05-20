using Geolaxia.Models;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Planets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Geolaxia.Controllers
{
    public class PlanetController : ApiController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PlanetController));
        private IPlanetService service;
        private IPlayerService playerService;

        public PlanetController(IPlanetService service, IPlayerService playerService)
        {
            this.service = service;
            this.playerService = playerService;
        }

        // GET api/planet/getbyplayer/
        [HttpGet]
        public JObject GetByPlayer()
        {
            string username = "";
            string token = "";
            var request = Request;
            var headers = request.Headers;
            if (!headers.Contains("username") || !headers.Contains("token"))
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "Sesión invalida" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }
            username = headers.GetValues("username").First();
            token = headers.GetValues("token").First();

            logger.Info("getting planets for player " + username);

            if (!playerService.ValidateToken(username, token)) 
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "Token invalido" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            try
            {
                var planets = service.GetByPlayer(username);
                var okResponse = new ApiResponse { Data = planets, Status = new Status { Result = "ok", Description = "" } };
                var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None));
                return json;
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                var responseError = new ApiResponse { Status = new Status { Result = "error", Description = "Ocurrio un error al obtener sus planetas. Intente nuevamente" } };
                return JObject.Parse(JsonConvert.SerializeObject(responseError, Formatting.None));
            }

        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}