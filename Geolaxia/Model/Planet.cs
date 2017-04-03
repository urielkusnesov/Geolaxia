using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class Planet
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Player Conqueror { get; set; }
        public virtual int Crystal { get; set; }
        public virtual int Metal { get; set; }
        public virtual int DarkMatter { get; set; }
        public virtual int Energy { get; set; }
        public virtual bool IsOrigin { get; set; }
        public virtual SolarSystem SolarSystem { get; set; }
    }
}
