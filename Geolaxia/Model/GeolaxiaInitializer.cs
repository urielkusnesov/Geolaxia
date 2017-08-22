using System.Collections.Generic;
using Model.Enum;

namespace Model
{
    public class GeolaxiaInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<GeolaxiaContext>
    {
        protected override void Seed(GeolaxiaContext context)
        {
            var players = new List<Player> { 
                new Player { Id = 1, UserName = "Uriel", FirstName = "Uriel", LastName = "Kusnesov", Password = "pass1" },
                new Player { Id = 2, UserName = "Ramiro", FirstName = "Ramiro", LastName = "Doi", Password = "pass2" }
            };

            foreach (Player player in players)
            {
                context.Players.Add(player);
            }

            var galaxies = new List<Galaxy> { 
                new Galaxy { Id = 1, Name = "Galaxy" }
            };

            foreach (Galaxy galaxy in galaxies)
            {
                context.Galaxies.Add(galaxy);
            }

            var solarSystems = new List<SolarSystem> { new SolarSystem { Id = 1, Name = "Solar System", Galaxy = galaxies[0] } };
            
            foreach(SolarSystem solarSystem in solarSystems) 
            {
                context.SolarSystems.Add(solarSystem); 
            }

            var planets = new List<Planet> { 
                new WhitePlanet { Id= 1, PlanetType = PlanetType.White, Name = "White", Conqueror = players[0], Crystal = 100, Metal = 200, DarkMatter = 300, Energy = 400, SolarSystem = solarSystems[0] },
                new BluePlanet { Id= 2, PlanetType = PlanetType.Blue, Name = "Blue", Conqueror = players[0], Crystal = 10, Metal = 20, DarkMatter = 30, Energy = 40, SolarSystem = solarSystems[0] },
                new BlackPlanet { Id= 3, PlanetType = PlanetType.Black, Name = "Black", Conqueror = players[1], Crystal = 150, Metal = 250, DarkMatter = 350, Energy = 450, SolarSystem = solarSystems[0] },
                new WhitePlanet { Id= 4, PlanetType = PlanetType.White, Name = "White2", Crystal = 100, Metal = 200, DarkMatter = 300, Energy = 400, SolarSystem = solarSystems[0] },
                new BluePlanet { Id= 5, PlanetType = PlanetType.Blue, Name = "Blue2", Crystal = 10, Metal = 20, DarkMatter = 30, Energy = 40, SolarSystem = solarSystems[0] },
                new BlackPlanet { Id= 6, PlanetType = PlanetType.Black, Name = "Black2", Crystal = 150, Metal = 250, DarkMatter = 350, Energy = 450, SolarSystem = solarSystems[0] },
                new WhitePlanet { Id= 7, PlanetType = PlanetType.White, Name = "White3", Crystal = 100, Metal = 200, DarkMatter = 300, Energy = 400, SolarSystem = solarSystems[0] },
                new BluePlanet { Id= 8, PlanetType = PlanetType.Blue, Name = "Blue3", Crystal = 10, Metal = 20, DarkMatter = 30, Energy = 40, SolarSystem = solarSystems[0] },
                new BlackPlanet { Id= 9, PlanetType = PlanetType.Black, Name = "Black3", Crystal = 150, Metal = 250, DarkMatter = 350, Energy = 450, SolarSystem = solarSystems[0] }
            };

            foreach (Planet planet in planets)
            {
                context.Planets.Add(planet);
            }

            var shipX = new ShipX { Id = 1, Name = "X1", Attack = 10, Planet = planets[0]};
            context.ShipsX.Add(shipX);

            context.SaveChanges();
        }
    }
}
