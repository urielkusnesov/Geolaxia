using Service.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using Model;

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

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            logger.Info("get players");
            IList<Player> players = service.List();
            return new string[] { players[0].UserName, players[1].UserName };
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