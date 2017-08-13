using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class AttackDTO
    {
        public int Id { get; set; }
        public int AttackerPlayerId { get; set; }
        public int AttackerPlanetId { get; set; }
        public int DestinationPlayerId { get; set; }
        public int DestinationPlanetId { get; set; }
        public List<int> FleetIds { get; set; }
        public DateTime FleetDeparture { get; set; }
        public DateTime FleetArrival { get; set; }
    }
}
