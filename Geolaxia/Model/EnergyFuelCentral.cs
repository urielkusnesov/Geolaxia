using Model.Enum;

namespace Model
{
    public class EnergyFuelCentral : EnergyFacility
    {
        public virtual int DarkMatterConsumption { get; set; }

        public EnergyFuelCentral()
        {
            this.EnergyFacilityType = EnergyFacilityType.EnergyFuelCentral;
        }

        public override void AddEnergy(Planet planet)
        {
            planet.Energy += Productivity;
        }
    }
}
