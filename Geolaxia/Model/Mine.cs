using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class Mine : Facility
    {
        public virtual int EnergyConsumption { get; set; }
    }
}
