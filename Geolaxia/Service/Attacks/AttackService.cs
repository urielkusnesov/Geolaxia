using System;
using System.Collections.Generic;
using Model;
using Model.DTO;
using Repository;
using Service.Planets;
using Service.Players;
using System.Linq;
using System.Timers;
using Service.Defenses;
using Service.Notification;
using Service.Ships;

namespace Service.Attacks
{
    public class AttackService : IAttackService
    {
        private readonly IRepositoryService repository;
        private IPlayerService playerService;
        private IPlanetService planetService;
        private INotificationService notificationService;

        public AttackService(IRepositoryService repository, IPlayerService playerService, IPlanetService planetService, INotificationService notificationService)
        {
            this.repository = repository;
            this.playerService = playerService;
            this.planetService = planetService;
            this.notificationService = notificationService;
        }

        public Attack Get(int id)
        {
            return repository.Get<Attack>(id);
        }

        public Attack Add(Attack attack)
        {
            var result = repository.Add<Attack>(attack);
            repository.SaveChanges();
            return result;
        }

        public Attack Remove(Attack attack)
        {
            var result = repository.Remove<Attack>(attack);
            repository.SaveChanges();
            return result;
        }

        public Attack Remove(int id)
        {
            var result = repository.Remove<Attack>(id);
            repository.SaveChanges();
            return result;
        }

        public Attack GetFromDTO(AttackDTO dto)
        {
            List<string> fleetIds = dto.FleetIds.Split(',').ToList();
            List<int> attackFleet = fleetIds.Select(fleet => Convert.ToInt32(fleet)).ToList();

            return new Attack
            {
                Id = dto.Id,
                AttackerPlayer = playerService.Get(dto.AttackerPlayerId),
                AttackerPlanet = planetService.Get(dto.DestinationPlanetId),
                DestinationPlayer = playerService.Get(dto.DestinationPlayerId),
                DestinationPlanet = planetService.Get(dto.DestinationPlanetId),
                Fleet = repository.List<Ship>(x => attackFleet.Contains(x.Id)).ToList(),
                FleetDeparture = dto.FleetDeparture,
                FleetArrival = dto.FleetArrival
            };
        }

        public void PerformAttack(Timer timer, Attack attack)
        {
            //destruyo el timer
            timer.Stop();
            timer.Dispose();

            //como el timer corre en otro thread no tengo el dbcontext instanciado
            using (var context = new GeolaxiaContext())
            {
                try
                {
                    var repo = new RepositoryService(context);
                    var attackFleet = attack.Fleet;
                    var aua = attackFleet.Sum(x => x.Attack);

                    var shipSv = new ShipService(repo);
                    var defenceSv = new DefenseService(repo);

                    var defenseFleet = shipSv.GetAvailableByPlanet(attack.DestinationPlanet.Id);
                    var defenseCanons = defenceSv.GetCanons(attack.DestinationPlanet.Id);
                    var aud = defenseFleet.Sum(x => x.Attack) + defenseCanons.Sum(x => x.Attack);

                    //rondas de ataque hasta que alguno pierda
                    while ((defenseCanons.Count == 0 && defenseFleet.Count ==0) || attackFleet.Count == 0)
                    {
                        AttackerMakeDamage(aua, ref defenseCanons, ref defenseFleet, repo);
                        DefenseMakeDamage(aud, ref attackFleet, repo);                        
                    }

                    repo.SaveChanges();

                    //notificacion push al usuario diciendo que se realizo el ataque y si gano o perdio
                    var player = repo.Get<Player>(x => x.Id == attack.AttackerPlayer.Id);
                    var attackerWon = aua >= defenseCanons.Sum(x => x.Defence) + defenseFleet.Sum(x => x.Defence);
                    notificationService.SendPushNotification(player.FirebaseToken, "Ataque realizado", attackerWon ? "Has derrotado al enemigo" : "Has sido derrotado", "HomeActivity");
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }

        private void AttackerMakeDamage(int aua, ref IList<Canon> defenseCanons, ref IList<Ship> defenseFleet, IRepositoryService repo)
        {
            //calculo cuanto pierde el defensor
            while (aua > 0)
            {
                //primero ataco canones
                foreach (var canon in defenseCanons)
                {
                    if (aua >= canon.Defence)
                    {
                        aua -= canon.Defence;
                        repo.Remove(canon);
                    }
                    else
                    {
                        var canonUpdated = repo.Get<Canon>(canon);
                        canonUpdated.Defence -= aua;
                        aua = 0;
                    }
                }

                //despues ataco naves
                foreach (var ship in defenseFleet.OrderBy(x => x.Defence))
                {
                    if (aua >= ship.Defence)
                    {
                        aua -= ship.Defence;
                        repo.Remove(ship);
                    }
                    else
                    {
                        var shipUpdated = repo.Get<Ship>(ship);
                        shipUpdated.Defence -= aua;
                        aua = 0;
                    }
                }
            }
        }

        private void DefenseMakeDamage(int aud, ref IList<Ship> attackFleet, IRepositoryService repo)
        {
            //calculo cuanto pierde el defensor
            while (aud > 0)
            {
                //ataco naves
                foreach (var ship in attackFleet.OrderBy(x => x.Defence))
                {
                    if (aud >= ship.Defence)
                    {
                        aud -= ship.Defence;
                        repo.Remove(ship);
                    }
                    else
                    {
                        var shipUpdated = repo.Get<Ship>(ship);
                        shipUpdated.Defence -= aud;
                        aud = 0;
                    }
                }
            }
        }

        //public List<long> GetAttacksList(int playerId)
        //{
        //    List<long> envios = new List<long>();

        //    IList<Attack> attacks = repository.List<Attack>(x => x.AttackerPlayer.Id.Equals(playerId) && x.FleetArrival > DateTime.Now);

        //    if (attacks != null && attacks.Count > 0)
        //    {
        //        foreach (var item in attacks)
        //        {
        //            long tiempo = this.GetMilli(item.FleetArrival);
        //            envios.Add(tiempo);
        //        }
        //    }

        //    return (envios);
        //}

        //public List<long> GetDefensesList(int playerId)
        //{
        //    List<long> envios = new List<long>();

        //    IList<Attack> defenses = repository.List<Attack>(x => x.DestinationPlayer.Id.Equals(playerId) && x.FleetArrival > DateTime.Now);

        //    if (defenses != null && defenses.Count > 0)
        //    {
        //        foreach (var item in defenses)
        //        {
        //            long tiempo = this.GetMilli(item.FleetArrival);
        //            envios.Add(tiempo);
        //        }
        //    }

        //    return (envios);
        //}

        public IList<Attack> GetAttacksList(int playerId)
        {
            IList<Attack> attacks = repository.List<Attack>(x => x.AttackerPlayer.Id.Equals(playerId) && x.FleetArrival > DateTime.Now);
            
            return (attacks);
        }

        public IList<Attack> GetDefensesList(int playerId)
        {
            IList<Attack> defenses = repository.List<Attack>(x => x.DestinationPlayer.Id.Equals(playerId) && x.FleetArrival > DateTime.Now);

            return (defenses);
        }

        private long GetMilli(DateTime date)
        {
            return (Convert.ToInt64(date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds));
        }
    }
}
