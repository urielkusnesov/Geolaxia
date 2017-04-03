using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class GeolaxiaInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<GeolaxiaContext>
    {
        protected override void Seed(GeolaxiaContext context)
        {
            var players = new List<Player> { new Player { Id = 1, UserName = "Uriel" }, new Player { Id = 2, UserName = "Ramiro" } };
            foreach (Player player in players)
            {
                context.Players.Add(player);
            }

            var shipX = new ShipX { Id = 1, Name = "X1", Attack = 10 };
            context.ShipsX.Add(shipX);

            context.SaveChanges();
        }
    }
}
