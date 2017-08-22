using System.Collections.Generic;

namespace Model
{
    public class Galaxy
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<SolarSystem> SolarSystems { get; set; }
    }
}
