using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SolarSystem
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Galaxy Galaxy { get; set; }
        public virtual IList<Planet> Planets { get; set; }
    }
}
