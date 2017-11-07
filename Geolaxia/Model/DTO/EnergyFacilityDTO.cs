using Model.Enum;

namespace Model.DTO
{
    public class EnergyFacilityDTO
    {
        public int Id { get; set; }
        public int ConstructionTime { get; set; }
        public int CostId { get; set; }
        public int Level { get; set; }
        public int PlanetId { get; set; }
        public int Productivity { get; set; }
        public int DarkMatterConsumption { get; set; }
        public EnergyFacilityType EnergyFacilityType { get; set; }
    }
}
