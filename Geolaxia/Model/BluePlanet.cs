using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Enum;

namespace Model
{
    public class BluePlanet : Planet
    {
        public BluePlanet() {
            this.PlanetType = PlanetType.Blue;
        }
    }
}
