using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SolarPanel : Facility
    {
        public virtual int CloudyProd { get; set; }
        public virtual int SunnyProd { get; set; }
        public virtual int RainyProd { get; set; }
    }
}
