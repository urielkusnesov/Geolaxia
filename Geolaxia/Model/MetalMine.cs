using Model.Enum;

namespace Model
{
    public class MetalMine : Mine
    {
        public MetalMine()
        {
            this.MineType = MineType.Metal;
        }

        public override void AddResource(Planet planet)
        {
            planet.AddMetal(Productivity);
            planet.Energy -= EnergyConsumption;
        }
    }
}
