using Model;
using Model.DTO;
using Repository;
using Service.Planets;
using Service.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Attacks
{
    public class AttackService : IAttackService
    {
        private readonly IRepositoryService repository;
        private IPlayerService playerService;
        private IPlanetService planetService;

        public AttackService(IRepositoryService repository, IPlayerService playerService, IPlanetService planetService)
        {
            this.repository = repository;
            this.playerService = playerService;
            this.planetService = planetService;
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
            return new Attack
            {
                Id = dto.Id,
                AttackerPlayer = playerService.Get(dto.AttackerPlayerId),
                AttackerPlanet = planetService.Get(dto.DestinationPlanetId),
                DestinationPlayer = playerService.Get(dto.DestinationPlayerId),
                DestinationPlanet = planetService.Get(dto.DestinationPlanetId),
                Fleet = repository.List<Ship>(x => dto.FleetIds.Contains(x.Id)).ToList(),
                FleetDeparture = dto.FleetDeparture,
                FleetArrival = dto.FleetArrival
            };
        }
    }
}
