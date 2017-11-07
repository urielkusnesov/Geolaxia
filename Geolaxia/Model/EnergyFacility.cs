using Model.Enum;

namespace Model
{
    public abstract class EnergyFacility : Facility
    {
        public EnergyFacilityType EnergyFacilityType { get; set; }

        public abstract void AddEnergy(Planet planet);
    }
}
