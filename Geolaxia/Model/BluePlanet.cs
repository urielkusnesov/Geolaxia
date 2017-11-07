using Model.Enum;

namespace Model
{
    public class BluePlanet : Planet
    {
        public BluePlanet() {
            this.PlanetType = PlanetType.Blue;
        }

        public override void AddCrystal(int productivity)
        {
            Crystal += productivity;
        }

        public override void AddMetal(int productivity)
        {
            Metal += (productivity * 1.10);
        }

        public override void AddDarkMatter(int productivity)
        {
            DarkMatter += productivity;
        }
    }
}
