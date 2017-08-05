using Geolaxia.Models;
using log4net;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Attacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Geolaxia.Controllers
{
    public class AttackController : ApiController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PlanetController));
        private IAttackService service;

        public AttackController(IAttackService service)
        {
            this.service = service;
        }

        // POST api/attack/attack (json attack in body)
        [HttpPost]
        public JObject Attack(Attack attack)
        {
            logger.Info("adding new attack");
            try
            {
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
    }
}
