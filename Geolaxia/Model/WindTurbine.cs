using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class WindTurbine : Facility
    {
        public virtual int Threshold { get; set; }
        public virtual int BestProd { get; set; }
        public virtual int WorstProd { get; set; }
    }
}
