using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Canon : Military
    {
        public virtual int Attack { get; set; }
        public virtual int Defence { get; set; }
    }
}
