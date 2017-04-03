using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Trader : Ship
    {
        public virtual int MaxCrystal { get; set; }
        public virtual int MaxMetal { get; set; }
        public virtual int MaxDarkMatter { get; set; }
    }
}
