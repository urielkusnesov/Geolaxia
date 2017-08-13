using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
