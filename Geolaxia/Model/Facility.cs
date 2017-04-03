using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class Facility
    {
        public virtual int Id { get; set; }
        public virtual double ConstructionTime { get; set; }
        public virtual Cost Cost { get; set; }
        public virtual int Level { get; set; }
        public virtual Planet Planet { get; set; }
        public virtual int Productivity { get; set; }
    }
}
