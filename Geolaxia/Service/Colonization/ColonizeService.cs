using System;
using System.Collections.Generic;
using Model;
using Repository;
using System.Linq;

namespace Service.Colonization
{
    public class ColonizeService : IColonizeService
    {
        private readonly IRepositoryService repository;

        //private int CANON_CONST_TIEMPO = 3;
        //private int CANON_CONST_COSTO_METAL = 100;
        //private int CANON_CONST_COSTO_CRISTAL = 50;
        //private int CANON_ATAQUE = 50;
        //private int CANON_DEFENSA = 50;
        //private int PREGUNTAS_CANTIDAD_A_RESPONDER = 3;
        //private int PREGUNTAS_CANTIDAD_EN_BASE = 24; //tiene que ser la (cantidad + 1) por el random

        public ColonizeService(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public IList<Colonizer> GetColonizers(int planetId)
        {
            IList<Colonize> lista = repository.List<Colonize>(x => x.ColonizerPlanet.Id.Equals(planetId));
            List<int> idColonizadoresUsadosList = (lista != null) ? lista.Select(x => x.Colonizer.Id).ToList() : new List<int>();           

            IList<Colonizer> colonizadorList = repository.List<Colonizer>(x => x.Planet.Id.Equals(planetId) && x.EnableDate < DateTime.Now && !idColonizadoresUsadosList.Contains(x.Id));

            return (colonizadorList);
        }

        public void SendColonizer(int planetId, int planetIdTarget, long time)
        {
            //TODO: validar que el planeta este libre.
            //TODO: validar que cuando llegue el colonizador siga estando libre para colonizar.
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddMilliseconds(time).ToLocalTime();

            Planet planetOrigen = repository.Get<Planet>(planetId);
            Player playerOrigen = repository.Get<Player>(x => x.Planets.Contains(planetOrigen));
            Planet planetDestino = repository.Get<Planet>(planetIdTarget);
            Player playerDestino = repository.Get<Player>(x => x.Planets.Contains(planetDestino));

            IList<Colonize> lista = repository.List<Colonize>(x => x.ColonizerPlanet.Id.Equals(planetId));
            List<int> idColonizadoresUsadosList = (lista != null) ? lista.Select(x => x.Colonizer.Id).ToList() : new List<int>();   

            Colonizer colonizador = repository.Get<Colonizer>(x => x.Planet.Id.Equals(planetId) && x.EnableDate <= DateTime.Now && !idColonizadoresUsadosList.Contains(x.Id));

            repository.Add<Colonize>(new Colonize { ColonizerArrival = date, ColonizerDeparture = DateTime.Now, ColonizerPlanet = planetOrigen, ColonizerPlayer = playerOrigen, DestinationPlanet = planetDestino, DestinationPlayer = playerDestino, Colonizer = colonizador });

            repository.SaveChanges();
        }

        public long IsSendingColonizer(int planetId)
        {
            long sendingTime = this.GetMilli(DateTime.MinValue);

            Colonize colonization = repository.Get<Colonize>(x => x.ColonizerPlanet.Id.Equals(planetId) && x.ColonizerArrival > DateTime.Now);

            if (colonization != null)
            {
                sendingTime = this.GetMilli(colonization.ColonizerArrival);
            }

            return (sendingTime);
        }

        private long GetMilli(DateTime date)
        {
            return (Convert.ToInt64(date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds));
        }
    }
}
