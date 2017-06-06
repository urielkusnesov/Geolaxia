using Service.Planets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using Model;
using Newtonsoft.Json;
using Geolaxia.Models;
using System.Web.Helpers;
using Newtonsoft.Json.Linq;

namespace Geolaxia.Controllers
{
    public class PlayerController : ApiController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PlayerController));
        private IPlayerService service;

        public PlayerController(IPlayerService service)
        {
            this.service = service;
        }

        // GET api/player/getall
        public IEnumerable<string> GetAll()
        {
            logger.Info("get all players");
            IList<Player> players = service.List();
            return new string[] { players[0].UserName, players[1].UserName };
        }

        // GET api/player/getbyid/id
        public string GetById(int id)
        {
            logger.Info("get player id: " + id.ToString());
            Player player = service.Get(id);
            return player.UserName;
        }

        // GET api/player/login/
        [HttpGet]    
        public JObject Login()
        {
            var username = "";
            var password = "";

            var request = Request;
            var headers = request.Headers;
            if (!headers.Contains("username") || !headers.Contains("password"))
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "Request invalido" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            username = headers.GetValues("username").First();
            password = headers.GetValues("password").First();

            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                logger.Info("loging in for user: " + username);
                var player = service.GetByUsername(username);
                if (player != null)
                {
                    logger.Info(username + " logged in succesfully");
                    string token = System.Guid.NewGuid().ToString();
                    //save the new token
                    player.Token = token;
                    player = service.Update(player.Id, player);
                    if (player != null)
                    {
                        var okResponse = new ApiResponse { Data = player, Status = new Status { Result = "ok", Description = "" } };
                        var json = JObject.Parse(JsonConvert.SerializeObject(okResponse, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore}));
                        return json;
                    }
                }
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "Usuario o contraseña incorrectos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            var responseError = new ApiResponse { Status = new Status { Result = "error", Description = "Ocurrio un error validando el usuario. Intente nuevamente" } };
            return JObject.Parse(JsonConvert.SerializeObject(responseError, Formatting.None));
        }

        // POST api/player/register (json player in body)
        public JObject Register(Player player)
        {
            logger.Info("registering new player");
            try
            {
                var newPlayer = service.Add(player);
                if (newPlayer != null)
                {
                    logger.Info(player.UserName + " registered succesfully");
                    var response = new ApiResponse { Status = new Status { Result = "ok", Description = "" } };
                    JObject json = JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                    return json;
                }
                else
                {
                    var response = new ApiResponse { Status = new Status { Result = "error", Description = "No se pudo crear el usuario. Intente nuevamente"} };
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

        // PUT api/player/update/id (json player in body)
        public void Update(int id, Player player)
        {
        }

        // DELETE api/player/delete/id
        public void Delete(int id)
        {
        }
    }
}