using Model.Enum;

namespace Model
{
    public class BlackPlanet : Planet
    {
        public BlackPlanet() {
            this.PlanetType = PlanetType.Black;
        }

        public override void AddCrystal(int productivity)
        {
            Crystal += (productivity*0.5);
        }

        public override void AddMetal(int productivity)
        {
            Metal += productivity;
        }

        public override void AddDarkMatter(int productivity)
        {
            if (Order == 9)
            {
                Crystal += (productivity * 1.15);
            }
            else if (Order == 8)
            {
                Crystal += (productivity * 1.10);
            }
            else
            {
                Crystal += (productivity * 1.05);
            }
        }
    }
}
