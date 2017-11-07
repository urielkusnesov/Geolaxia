using Model.Enum;

namespace Model
{
    public class SolarPanel : EnergyFacility
    {
        public virtual int CloudyProd { get; set; }
        public virtual int SunnyProd { get; set; }
        public virtual int RainyProd { get; set; }

        public SolarPanel()
        {
            this.EnergyFacilityType = EnergyFacilityType.SolarPanel;
        }

        public override void AddEnergy(Planet planet)
        {
            switch (planet.Conqueror.WeatherDesc)
            {
                case WeatherDesc.Cloudy:
                    planet.Energy += CloudyProd;
                    break;
                case WeatherDesc.Sunny:
                    planet.Energy += SunnyProd;
                    break;
                case WeatherDesc.Rainy:
                    planet.Energy += RainyProd;
                    break;
            }
        }
    }
}
