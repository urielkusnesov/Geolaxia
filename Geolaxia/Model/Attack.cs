﻿using System;
using System.Collections.Generic;

namespace Model
{
    public class Attack
    {
        public virtual int Id { get; set; }
        public virtual Player AttackerPlayer { get; set; }
        public virtual Planet AttackerPlanet { get; set; }
        public virtual Player DestinationPlayer { get; set; }
        public virtual Planet DestinationPlanet { get; set; }
        public virtual IList<Ship> Fleet { get; set; }
        public virtual DateTime FleetDeparture { get; set; }
        public virtual DateTime FleetArrival { get; set; }
    }
}
