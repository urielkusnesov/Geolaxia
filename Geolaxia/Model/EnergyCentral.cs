using Model.Enum;

namespace Model
{
    public class EnergyCentral : EnergyFacility
    {
        public EnergyCentral()
        {
            EnergyFacilityType = EnergyFacilityType.EnergyCentral;
        }

        public override void AddEnergy(Planet planet)
        {
            planet.Energy += Productivity;
        }
    }
}
