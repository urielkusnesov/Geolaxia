using Model;
using Model.Enum;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Planets
{
    public class PlanetService : IPlanetService
    {
        private readonly IRepositoryService repository;
        private static Random rnd = new Random();

        public PlanetService(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public Planet Get(int id)
        {
            return repository.Get<Planet>(id);
        }

        public IList<Planet> GetByPlayer(string username)
        {
            var planets = repository.List<Planet>(x => x.Conqueror.UserName == username);
            return planets;
        }

        public IList<Planet> GetBySolarSystem(int solarSystemId)
        {
            var planets = repository.List<Planet>(x => x.SolarSystem.Id == solarSystemId).OrderBy(x => x.Order).ToList();
            return planets;
        }

        public Planet GetRandomFreePlanet()
        {
            var planets = repository.List<Planet>(x => x.Conqueror == null);
            if (planets.Count > 0)
            {
                int random = rnd.Next(planets.Count);
                return planets[random];
            }
            else 
            {
                AddNewSolarSystem();
                return GetRandomFreePlanet();
            }
        }

        private void AddNewSolarSystem()
        {
            var galaxy = repository.Get<Galaxy>(1);
            
            var solarSystemsCount = repository.Count<SolarSystem>();
            var solarSystem = new SolarSystem { Name = "Sistema Solar " + solarSystemsCount.ToString(), Galaxy = galaxy };
            repository.Add<SolarSystem>(solarSystem);

            var planets = new List<Planet> { 
                new WhitePlanet { Id= 1, PlanetType = PlanetType.White, Name = "White", Crystal = 100, Metal = 200, DarkMatter = 300, Energy = 400, SolarSystem = solarSystem },
                new BluePlanet { Id= 2, PlanetType = PlanetType.Blue, Name = "Blue", Crystal = 10, Metal = 20, DarkMatter = 30, Energy = 40, SolarSystem = solarSystem },
                new BlackPlanet { Id= 3, PlanetType = PlanetType.Black, Name = "Black", Crystal = 150, Metal = 250, DarkMatter = 350, Energy = 450, SolarSystem = solarSystem },
                new WhitePlanet { Id= 4, PlanetType = PlanetType.White, Name = "White2", Crystal = 100, Metal = 200, DarkMatter = 300, Energy = 400, SolarSystem = solarSystem },
                new BluePlanet { Id= 5, PlanetType = PlanetType.Blue, Name = "Blue2", Crystal = 10, Metal = 20, DarkMatter = 30, Energy = 40, SolarSystem = solarSystem },
                new BlackPlanet { Id= 6, PlanetType = PlanetType.Black, Name = "Black2", Crystal = 150, Metal = 250, DarkMatter = 350, Energy = 450, SolarSystem = solarSystem },
                new WhitePlanet { Id= 7, PlanetType = PlanetType.White, Name = "White3", Crystal = 100, Metal = 200, DarkMatter = 300, Energy = 400, SolarSystem = solarSystem },
                new BluePlanet { Id= 8, PlanetType = PlanetType.Blue, Name = "Blue3", Crystal = 10, Metal = 20, DarkMatter = 30, Energy = 40, SolarSystem = solarSystem },
                new BlackPlanet { Id= 9, PlanetType = PlanetType.Black, Name = "Black3", Crystal = 150, Metal = 250, DarkMatter = 350, Energy = 450, SolarSystem = solarSystem }
            };

            foreach (Planet planet in planets)
            {
                repository.Add<Planet>(planet);
            }
        }

        public IList<Galaxy> GetAllGalaxies()
        {
            return repository.List<Galaxy>();
        }

        public IList<SolarSystem> GetSolarSystemsByGalaxy(int galaxyId)
        {
            return repository.List<SolarSystem>(x => x.Galaxy.Id == galaxyId);
        }

        public void UseResources(int planetId, Cost cost)
        {
            var planet = repository.Get<Planet>(planetId);
            planet.Crystal -= cost.CrystalCost;
            planet.Metal -= cost.MetalCost;
            planet.DarkMatter -= cost.DarkMatterCost;

            repository.SaveChanges();
        }
    }
}
