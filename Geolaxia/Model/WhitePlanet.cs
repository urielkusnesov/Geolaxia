using Model.Enum;

namespace Model
{
    public class WhitePlanet : Planet
    {
        public WhitePlanet() {
            this.PlanetType = PlanetType.White;
        }

        public override void AddCrystal(int productivity)
        {
            if (Order == 1)
            {
                Crystal += (productivity*1.15);
            }
            else if(Order == 2)
            {
                Crystal += (productivity * 1.10);                
            }
            else
            {
                Crystal += (productivity * 1.05);
            }
        }

        public override void AddMetal(int productivity)
        {
            Metal += productivity;
        }

        public override void AddDarkMatter(int productivity)
        {
            DarkMatter += (productivity*0.5);
        }
    }
}
