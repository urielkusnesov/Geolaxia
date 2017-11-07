using Model.Enum;

namespace Model
{
    public class DarkMatterMine : Mine
    {
        public DarkMatterMine()
        {
            this.MineType = MineType.DarkMatter;
        }

        public override void AddResource(Planet planet)
        {
            planet.AddDarkMatter(Productivity);
            planet.Energy -= EnergyConsumption;
        }
    }
}
