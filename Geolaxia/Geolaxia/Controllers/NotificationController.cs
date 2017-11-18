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
using Service.Colonization;

namespace Geolaxia.Controllers
{
    public class NotificationController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(NotificationController));
        private IPlayerService playerService;
        private IAttackService attackService;
        private IColonizeService colonizeService;

        private static System.Timers.Timer aTimer;

        public NotificationController(IPlayerService playerService, IAttackService attackService, IColonizeService colonizeService)
            : base(playerService)
        {
            this.attackService = attackService;
            this.colonizeService = colonizeService;
        }

        //api/notification/GetNotifications
        [HttpPost]
        public JObject GetNotifications(int playerId)
        {
            if (!ValidateToken())
            {
                var response = new ApiResponse { Status = new Status { Result = "error", Description = "datos de sesión invalidos" } };
                return JObject.Parse(JsonConvert.SerializeObject(response, Formatting.None));
            }

            logger.Info("getting notifications for player: " + playerId);

            try
            {
                IList<long> colo = colonizeService.GetColonizesList(playerId);
                IList<long> att = attackService.GetAttacksList(playerId);
                IList<long> def = attackService.GetDefensesList(playerId);

                IList<Notification> notifications = new List<Notification>();

                foreach (var item in colo)
                {
                    notifications.Add(new Notification("Colonización", item));
                }

                foreach (var item in att)
                {
                    notifications.Add(new Notification("Ataque", item));
                }

                foreach (var item in def)
                {
                    notifications.Add(new Notification("Defensa", item));
                }

                var okResponse = new ApiResponse { Data = notifications, Status = new Status { Result = "ok", Description = "" } };
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

    class Notification
    {
        public string TipoNotificacion { get; set; }
        public long Time { get; set; }

        public Notification(string tipo, long tiempo)
        {
            this.TipoNotificacion = tipo;
            this.Time = tiempo;
        }
    }
}
