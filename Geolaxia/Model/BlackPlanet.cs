using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Enum;

namespace Model
{
    public class BlackPlanet : Planet
    {
        public BlackPlanet() {
            this.PlanetType = PlanetType.Black;
        }
    }
}
