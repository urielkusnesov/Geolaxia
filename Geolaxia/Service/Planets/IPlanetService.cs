using Model;
using System.Collections.Generic;
using Model.DTO;

namespace Service.Planets
{
    public interface IPlanetService
    {
        Planet Get(int id);

        IList<Planet> GetByPlayer(string username);

        IList<Planet> GetBySolarSystem(int solarSystemId);

        Planet GetRandomFreePlanet();

        IList<Galaxy> GetAllGalaxies();

        IList<SolarSystem> GetSolarSystemsByGalaxy(int galaxyId);

        void UseResources(int planetId, Cost cost);

        void AddMineResources(MineDTO mineDto);

        void AddEnergy(EnergyFacilityDTO energyFacilityDto);
    }
}
