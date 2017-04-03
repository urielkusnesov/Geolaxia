using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Galaxy
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<SolarSystem> SolarSystems { get; set; }
    }
}
