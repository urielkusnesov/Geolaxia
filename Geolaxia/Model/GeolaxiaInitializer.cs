using System;
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

            var costs = new List<Cost> {
                new Cost { Id = 1, Element = "crystal mine L1", CrystalCost = 100, MetalCost = 100 },
                new Cost { Id = 2, Element = "crystal mine L2", CrystalCost = 200, MetalCost = 200 },
                new Cost { Id = 3, Element = "crystal mine L3", CrystalCost = 300, MetalCost = 300 },
                new Cost { Id = 4, Element = "crystal mine L4", CrystalCost = 400, MetalCost = 400 },
                new Cost { Id = 5, Element = "crystal mine L5", CrystalCost = 500, MetalCost = 500 },
                new Cost { Id = 6, Element = "metal mine L1", CrystalCost = 100, MetalCost = 100 },
                new Cost { Id = 7, Element = "metal mine L2", CrystalCost = 150, MetalCost = 150 },
                new Cost { Id = 8, Element = "metal mine L3", CrystalCost = 200, MetalCost = 200 },
                new Cost { Id = 9, Element = "metal mine L4", CrystalCost = 250, MetalCost = 250 },
                new Cost { Id = 10, Element = "metal mine L5", CrystalCost = 300, MetalCost = 300 },
                new Cost { Id = 11, Element = "dark matter mine L1", CrystalCost = 100, MetalCost = 100 },
                new Cost { Id = 12, Element = "dark matter mine L2", CrystalCost = 300, MetalCost = 300 },
                new Cost { Id = 13, Element = "dark matter mine L3", CrystalCost = 500, MetalCost = 500 },
                new Cost { Id = 14, Element = "dark matter mine L4", CrystalCost = 700, MetalCost = 700 },
                new Cost { Id = 15, Element = "dark matter mine L5", CrystalCost = 900, MetalCost = 900 },
                new Cost { Id = 16, Element = "energy central L1", CrystalCost = 25, MetalCost = 100 },
                new Cost { Id = 17, Element = "energy central L2", CrystalCost = 50, MetalCost = 200 },
                new Cost { Id = 18, Element = "energy central L3", CrystalCost = 75, MetalCost = 300 },
                new Cost { Id = 19, Element = "energy central L4", CrystalCost = 100, MetalCost = 400 },
                new Cost { Id = 20, Element = "energy central L5", CrystalCost = 125, MetalCost = 500 },
                new Cost { Id = 21, Element = "fuel energy central L1", CrystalCost = 25, MetalCost = 100 },
                new Cost { Id = 22, Element = "fuel energy central L2", CrystalCost = 50, MetalCost = 200 },
                new Cost { Id = 23, Element = "fuel energy central L3", CrystalCost = 75, MetalCost = 300 },
                new Cost { Id = 24, Element = "fuel energy central L4", CrystalCost = 100, MetalCost = 400 },
                new Cost { Id = 25, Element = "fuel energy central L5", CrystalCost = 125, MetalCost = 500 },
                new Cost { Id = 26, Element = "solar panel", CrystalCost = 5, MetalCost = 20 },
                new Cost { Id = 27, Element = "wind turbine", CrystalCost = 5, MetalCost = 20 },
                new Cost { Id = 28, Element = "militar factory", CrystalCost = 2000, MetalCost = 2000, DarkMatterCost = 500},
                new Cost { Id = 29, Element = "ship x", CrystalCost = 50, MetalCost = 250 },
                new Cost { Id = 30, Element = "ship y", CrystalCost = 75, MetalCost = 500 },
                new Cost { Id = 31, Element = "ship z", CrystalCost = 100, MetalCost = 1000 },
                new Cost { Id = 32, Element = "canon", CrystalCost = 50, MetalCost = 100 },
                new Cost { Id = 33, Element = "shield", CrystalCost = 5000, MetalCost = 5000 },
                new Cost { Id = 34, Element = "probe", CrystalCost = 3000, MetalCost = 3000 },
                new Cost { Id = 35, Element = "ship z", CrystalCost = 500, MetalCost = 500 }
            };

            foreach (Cost cost in costs)
            {
                context.Costs.Add(cost);
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
                new WhitePlanet { Id= 1, PlanetType = PlanetType.White, Name = "White", Order = 1, Conqueror = players[0], Crystal = 100, Metal = 200, DarkMatter = 300, Energy = 400, SolarSystem = solarSystems[0] },
                new BluePlanet { Id= 2, PlanetType = PlanetType.Blue, Name = "Blue", Order = 2, Conqueror = players[0], Crystal = 10, Metal = 20, DarkMatter = 30, Energy = 40, SolarSystem = solarSystems[0] },
                new BlackPlanet { Id= 3, PlanetType = PlanetType.Black, Name = "Black", Order = 3, Conqueror = players[1], Crystal = 150, Metal = 250, DarkMatter = 350, Energy = 450, SolarSystem = solarSystems[0] },
                new WhitePlanet { Id= 4, PlanetType = PlanetType.White, Name = "White2", Order = 4, Crystal = 100, Metal = 200, DarkMatter = 300, Energy = 400, SolarSystem = solarSystems[0] },
                new BluePlanet { Id= 5, PlanetType = PlanetType.Blue, Name = "Blue2", Order = 5, Crystal = 10, Metal = 20, DarkMatter = 30, Energy = 40, SolarSystem = solarSystems[0] },
                new BlackPlanet { Id= 6, PlanetType = PlanetType.Black, Name = "Black2", Order = 6, Crystal = 150, Metal = 250, DarkMatter = 350, Energy = 450, SolarSystem = solarSystems[0] },
                new WhitePlanet { Id= 7, PlanetType = PlanetType.White, Name = "White3", Order = 7, Crystal = 100, Metal = 200, DarkMatter = 300, Energy = 400, SolarSystem = solarSystems[0] },
                new BluePlanet { Id= 8, PlanetType = PlanetType.Blue, Name = "Blue3", Order = 8, Crystal = 10, Metal = 20, DarkMatter = 30, Energy = 40, SolarSystem = solarSystems[0] },
                new BlackPlanet { Id= 9, PlanetType = PlanetType.Black, Name = "Black3", Order = 9, Crystal = 150, Metal = 250, DarkMatter = 350, Energy = 450, SolarSystem = solarSystems[0] }
            };

            foreach (Planet planet in planets)
            {
                context.Planets.Add(planet);
            }

            var ships = new List<Ship>
            {
                new ShipX { Id = 1, Name = "X1", Attack = 10, Defence = 20, Planet = planets[0], Speed = 10, Cost = costs[28], EnableDate = DateTime.Now},
                new ShipX { Id = 2, Name = "X2", Attack = 10, Defence = 30, Planet = planets[3], Speed = 10, Cost = costs[29], EnableDate = DateTime.Now}
            };

            foreach (Ship ship in ships)
            {
                context.Ships.Add(ship);
            }

            var canon = new Canon{Id = 1, Attack = 50, Defence = 50, Cost = costs[31], Planet = planets[0], Name = "C1", ConstructionTime = 3, RequiredLevel = 2, EnableDate = DateTime.Now};
            context.Canons.Add(canon);

            var shield = new Shield { Id = 1, ConstructionTime = 3, Cost = costs[32], EnableDate = DateTime.Now, Name = "Shield1", Planet = planets[0], RequiredLevel = 1 };
            context.Shields.Add(shield);

            context.SaveChanges();
        }
    }
}
