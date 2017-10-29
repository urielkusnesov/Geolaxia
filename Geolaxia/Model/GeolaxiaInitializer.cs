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

            var canon = new Canon{Id = 1, Attack = 50, Defence = 50, Cost = costs[31], Planet = planets[0], ConstructionTime = 3, RequiredLevel = 2, EnableDate = DateTime.Now};
            context.Canons.Add(canon);

            var shield = new Shield { Id = 1, ConstructionTime = 3, Cost = costs[32], EnableDate = DateTime.Now, Planet = planets[0], RequiredLevel = 1 };
            context.Shields.Add(shield);

            context.SaveChanges();

            this.CargarPreguntasYRespuestasParaDefensa(context);
        }

        private void CargarPreguntasYRespuestasParaDefensa(GeolaxiaContext context)
        {
            List<string> respuestas = new List<string>();
            Queztion question = null;

            //Pregunta 1
            question = new Queztion();
            question.Question = "¿Cuál es el planeta más grande del sistema solar?";
            respuestas.Clear();
            respuestas.Add("Tierra");
            respuestas.Add("Marte");
            respuestas.Add("Sol");
            respuestas.Add("Júpiter");
            respuestas.Add("Saturno");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "Júpiter";
            context.Questions.Add(question);

            //Pregunta 2
            question = new Queztion();
            question.Question = "¿Cuál es el planeta más pequeño del sistema solar?";
            respuestas.Clear();
            respuestas.Add("Tierra");
            respuestas.Add("Mercurio");
            respuestas.Add("Plutón");
            respuestas.Add("Júpiter");
            respuestas.Add("Venus");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "Mercurio";
            context.Questions.Add(question);

            //Pregunta 3
            question = new Queztion();
            question.Question = "¿Cuál es el planeta del sistema solar ubicado en la tercera posición?";
            respuestas.Clear();
            respuestas.Add("Tierra");
            respuestas.Add("Marte");
            respuestas.Add("Urano");
            respuestas.Add("Neptuno");
            respuestas.Add("Mercurio");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "Tierra";
            context.Questions.Add(question);

            //Pregunta 4
            question = new Queztion();
            question.Question = "¿Cada cuántos años pasa el cometa Halley?";
            respuestas.Clear();
            respuestas.Add("75 años");
            respuestas.Add("55 años");
            respuestas.Add("100 años");
            respuestas.Add("20 años");
            respuestas.Add("60 años");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "75 años";
            context.Questions.Add(question);

            //Pregunta 5
            question = new Queztion();
            question.Question = "¿En qué año el hombre pisó la Luna?";
            respuestas.Clear();
            respuestas.Add("1962");
            respuestas.Add("1969");
            respuestas.Add("1973");
            respuestas.Add("1979");
            respuestas.Add("1968");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "1969";
            context.Questions.Add(question);

            //Pregunta 6
            question = new Queztion();
            question.Question = "¿Cuál fue el nombre del primer hombre en pisar la Luna?";
            respuestas.Clear();
            respuestas.Add("Lance Armstrong");
            respuestas.Add("Edwin Aldrin");
            respuestas.Add("Michael Collins");
            respuestas.Add("Neil Armstrong");
            respuestas.Add("Nil Armstrong");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "Neil Armstrong";
            context.Questions.Add(question);

            //Pregunta 7
            question = new Queztion();
            question.Question = "¿Cuál fue el nombre del segundo hombre en pisar la Luna?";
            respuestas.Clear();
            respuestas.Add("Lance Armstrong");
            respuestas.Add("Edwin Aldrin");
            respuestas.Add("Michael Collins");
            respuestas.Add("Neil Armstrong");
            respuestas.Add("Nil Armstrong");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "Edwin Aldrin";
            context.Questions.Add(question);

            //Pregunta 8
            question = new Queztion();
            question.Question = "¿Cuál fue el número de APOLO en el cual se llegó a la Luna?";
            respuestas.Clear();
            respuestas.Add("5");
            respuestas.Add("10");
            respuestas.Add("11");
            respuestas.Add("13");
            respuestas.Add("14");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "11";
            context.Questions.Add(question);
            
            //Pregunta 9
            question = new Queztion();
            question.Question = "¿Cuántas misiones “APOLO” se realizaron?";
            respuestas.Clear();
            respuestas.Add("11");
            respuestas.Add("13");
            respuestas.Add("10");
            respuestas.Add("20");
            respuestas.Add("17");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "17";
            context.Questions.Add(question);

            //Pregunta 10
            question = new Queztion();
            question.Question = "¿Cuál es la distancia de la Tierra con el Sol?";
            respuestas.Clear();
            respuestas.Add("12742 km");
            respuestas.Add("1.3914 millones km");
            respuestas.Add("30000 km");
            respuestas.Add("170 millones km");
            respuestas.Add("149.6 millones km");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "149.6 millones km";
            context.Questions.Add(question);

            //Pregunta 11
            question = new Queztion();
            question.Question = "¿Cuál es la distancia de la Tierra con la luna?";
            respuestas.Clear();
            respuestas.Add("384400 km");
            respuestas.Add("12742 km");
            respuestas.Add("450000 km");
            respuestas.Add("1 millón km");
            respuestas.Add("30000 km");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "384400 km";
            context.Questions.Add(question);

            //Pregunta 12
            question = new Queztion();
            question.Question = "¿Cuál es el diámetro de la Tierra?";
            respuestas.Clear();
            respuestas.Add("1.3914 millones km");
            respuestas.Add("3474 km");
            respuestas.Add("12742 km");
            respuestas.Add("3000 km");
            respuestas.Add("3.2 millones km");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "12742 km";
            context.Questions.Add(question);

            //Pregunta 13
            question = new Queztion();
            question.Question = "¿Cuál es el diámetro del Sol?";
            respuestas.Clear();
            respuestas.Add("3474 km");
            respuestas.Add("12742 km");
            respuestas.Add("3000 km");
            respuestas.Add("3.2 millones km");
            respuestas.Add("1.3914 millones km");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "1.3914 millones km";
            context.Questions.Add(question);

            //Pregunta 14
            question = new Queztion();
            question.Question = "¿Cuándo se estima que fue el Big Bang?";
            respuestas.Clear();
            respuestas.Add("65 millones de años");
            respuestas.Add("4570 millones de años");
            respuestas.Add("5011 millones de años");
            respuestas.Add("11.210 millones de años");
            respuestas.Add("13.810 millones de años");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "13.810 millones de años";
            context.Questions.Add(question);

            //Pregunta 15
            question = new Queztion();
            question.Question = "¿Cuántos movimientos realiza la Tierra alrededor del Sol?";
            respuestas.Clear();
            respuestas.Add("2");
            respuestas.Add("6");
            respuestas.Add("3");
            respuestas.Add("5");
            respuestas.Add("1");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "5";
            context.Questions.Add(question);

            //Pregunta 16
            question = new Queztion();
            question.Question = "¿Cuánto tarda la Tierra en dar una vuelta alrededor del Sol (en horas)?";
            respuestas.Clear();
            respuestas.Add("8760 horas");
            respuestas.Add("8754 horas");
            respuestas.Add("8766 horas");
            respuestas.Add("8770 horas");
            respuestas.Add("8768 horas");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "8766 horas";
            context.Questions.Add(question);

            //Pregunta 17
            question = new Queztion();
            question.Question = "¿Cuál es el planeta más cercano al sol de nuestro sistema solar?";
            respuestas.Clear();
            respuestas.Add("Tierra");
            respuestas.Add("Mercurio");
            respuestas.Add("Plutón");
            respuestas.Add("Júpiter");
            respuestas.Add("Venus");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "Mercurio";
            context.Questions.Add(question);

            //Pregunta 18
            question = new Queztion();
            question.Question = "¿Cuál es el cuarto planeta más cercano al sol de nuestro sistema solar (empezando a contar desde el Sol)?";
            respuestas.Clear();
            respuestas.Add("Tierra");
            respuestas.Add("Marte");
            respuestas.Add("Neptuno");
            respuestas.Add("Júpiter");
            respuestas.Add("Saturno");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "Marte";
            context.Questions.Add(question);
            
            //Pregunta 19
            question = new Queztion();
            question.Question = "¿Cuál es el octavo planeta más cercano al sol de nuestro sistema solar (empezando a contar desde el Sol)?";
            respuestas.Clear();
            respuestas.Add("Neptuno");
            respuestas.Add("Tierra");
            respuestas.Add("Marte");
            respuestas.Add("Júpiter");
            respuestas.Add("Saturno");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "Neptuno";
            context.Questions.Add(question);
            
            //Pregunta 20
            question = new Queztion();
            question.Question = "¿Cuánto tarda la Tierra en dar una vuelta sobre sí misma (en segundos)?";
            respuestas.Clear();
            respuestas.Add("123400");
            respuestas.Add("122500");
            respuestas.Add("122450");
            respuestas.Add("122400");
            respuestas.Add("132400");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "122400";
            context.Questions.Add(question);
            
            //Pregunta 21
            question = new Queztion();
            question.Question = "¿De qué forma es la trayectoria que realiza la Tierra alrededor del Sol?";
            respuestas.Clear();
            respuestas.Add("Circular");
            respuestas.Add("Lineal");
            respuestas.Add("Triangular");
            respuestas.Add("Elíptica");
            respuestas.Add("Aleatoria");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "Elíptica";
            context.Questions.Add(question);
            
            //Pregunta 22
            question = new Queztion();
            question.Question = "¿En qué año se fundó la NASA?";
            respuestas.Clear();
            respuestas.Add("1880");
            respuestas.Add("1930");
            respuestas.Add("1958");
            respuestas.Add("1962");
            respuestas.Add("1965");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "1958";
            context.Questions.Add(question);

            //Pregunta 23
            question = new Queztion();
            question.Question = "¿Dónde queda ubicada la central de la NASA?";
            respuestas.Clear();
            respuestas.Add("Los Ángeles");
            respuestas.Add("California");
            respuestas.Add("Washington DC");
            respuestas.Add("New York");
            respuestas.Add("Colorado");
            question.Answers = string.Join("|", respuestas);
            question.CorrectAnswer = "Washington DC";
            context.Questions.Add(question);

            context.SaveChanges();
        }
    }
}
