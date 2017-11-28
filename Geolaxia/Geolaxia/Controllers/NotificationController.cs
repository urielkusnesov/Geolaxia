using Geolaxia.Models;
using log4net;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Attacks;
using Service.Players;
using System;
using System.Collections.Generic;
using System.Web.Http;
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
                //IList<long> colo = colonizeService.GetColonizesList(playerId);
                IList<Colonize> colo = colonizeService.GetColonizesList(playerId);
                //IList<long> att = attackService.GetAttacksList(playerId);
                //IList<long> def = attackService.GetDefensesList(playerId);
                IList<Attack> att = attackService.GetAttacksList(playerId);
                IList<Attack> def = attackService.GetDefensesList(playerId);

                IList<Notification> notifications = new List<Notification>();

                foreach (var item in colo)
                {
                    notifications.Add(new NotificacionColonizacion("Colonización", 
                        this.GetMilli(item.ColonizerArrival), 
                        item.ColonizerPlayer.UserName, 
                        item.ColonizerPlanet.Name, 
                        string.Format("{0}-{1}", item.DestinationPlanet.Order, item.DestinationPlanet.Name)));
                }

                foreach (var item in att)
                {
                    //notifications.Add(new Notification("Ataque", item));
                    notifications.Add(new NotificacionAtaque("Ataque", 
                        this.GetMilli(item.FleetArrival), 
                        item.DestinationPlayer.UserName, 
                        string.Format("{0}-{1}", item.DestinationPlanet.Order, item.DestinationPlanet.Name),
                        string.Format("{0}-{1}", item.AttackerPlanet.Order, item.AttackerPlanet.Name)));
                }

                foreach (var item in def)
                {
                    //notifications.Add(new Notification("Defensa", item));
                    notifications.Add(new NotificacionDefensa("Defensa", 
                        this.GetMilli(item.FleetArrival), 
                        item.AttackerPlayer.UserName, 
                        string.Format("{0}-{1}", item.AttackerPlanet.Order, item.AttackerPlanet.Name), 
                        string.Format("{0}-{1}", item.DestinationPlanet.Order, item.DestinationPlanet)));
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

        private long GetMilli(DateTime date)
        {
            return (Convert.ToInt64(date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds));
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

    class NotificacionAtaque : Notification
    {
        public string PlayerName { get; set; }
        public string PlanetNameT { get; set; }
        public string PlanetNameO { get; set; }

        public NotificacionAtaque(string tipo, long tiempo, string nombre, string planetaAtacado, string planetaAtacante)
            : base(tipo, tiempo)
        {
            this.PlayerName = nombre;
            this.PlanetNameT = planetaAtacado;
            this.PlanetNameO = planetaAtacante;
        }
    }

    class NotificacionDefensa : Notification
    {
        public string PlayerName { get; set; }
        public string PlanetNameT { get; set; }
        public string PlanetNameO { get; set; }

        public NotificacionDefensa(string tipo, long tiempo, string nombre, string planetaAtacante, string planetaAtacado)
            : base(tipo, tiempo)
        {
            this.PlayerName = nombre;
            this.PlanetNameO = planetaAtacante;
            this.PlanetNameT = planetaAtacado;
        }
    }

    class NotificacionColonizacion : Notification
    {
        public string PlayerName { get; set; }
        public string PlanetNameT { get; set; }
        public string PlanetNameO { get; set; }

        public NotificacionColonizacion(string tipo, long tiempo, string nombre, string planetaOrigen, string planetaDestino)
            : base(tipo, tiempo)
        {
            this.PlayerName = nombre;
            this.PlanetNameO = planetaOrigen;
            this.PlanetNameT = planetaDestino;
        }
    }
}
