using Model.Enum;

namespace Model
{
    public class WindTurbine : EnergyFacility
    {
        public virtual int Threshold { get; set; }
        public virtual int BestProd { get; set; }
        public virtual int WorstProd { get; set; }

        public WindTurbine()
        {
            this.EnergyFacilityType = EnergyFacilityType.WindTurbine;
        }

        public override void AddEnergy(Planet planet)
        {
            if (planet.Conqueror.WeatherWindSpeed >= Threshold)
            {
                planet.Energy += BestProd;
            }
            else
            {
                planet.Energy += WorstProd;
            }
        }
    }
}
