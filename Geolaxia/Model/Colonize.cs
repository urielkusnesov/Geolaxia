using System;

namespace Model
{
    public class Colonize
    {
        public virtual int Id { get; set; }
        public virtual Player ColonizerPlayer { get; set; }
        public virtual Planet ColonizerPlanet { get; set; }
        public virtual Planet DestinationPlanet { get; set; }
        public virtual Probe Colonizer { get; set; }
        public virtual DateTime ColonizerDeparture { get; set; }
        public virtual DateTime ColonizerArrival { get; set; }
        public virtual bool MissionComplete { get; set; }
    }
}
