using Model.Enum;

namespace Model
{
    public class CrystalMine : Mine
    {
        public CrystalMine() 
        {
            this.MineType = MineType.Crystal;
        }

        public override void AddResource(Planet planet)
        {
            planet.AddCrystal(Productivity);
            planet.Energy -= EnergyConsumption;
        }
    }
}
