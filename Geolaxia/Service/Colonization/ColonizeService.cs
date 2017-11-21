using System;
using System.Collections.Generic;
using Model;
using Repository;
using System.Linq;
using System.Timers;

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

        public IList<Probe> GetColonizers(int planetId)
        {
            IList<Colonize> lista = repository.List<Colonize>(x => x.ColonizerPlanet.Id.Equals(planetId));
            List<int> idColonizadoresUsadosList = (lista != null) ? lista.Select(x => x.Colonizer.Id).ToList() : new List<int>();

            IList<Probe> colonizadorList = repository.List<Probe>(x => x.Planet.Id.Equals(planetId) && x.EnableDate < DateTime.Now && !idColonizadoresUsadosList.Contains(x.Id));

            return (colonizadorList);
        }

        public void SendColonizer(int planetId, int planetIdTarget, long time)
        {
            //TODO: validar que el planeta este libre.
            //TODO: validar que cuando llegue el colonizador siga estando libre para colonizar.
            //DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime arrival = DateTime.Now.AddMilliseconds(time).ToLocalTime();
            //DateTime date = start.AddMilliseconds(time).ToLocalTime();

            Planet planetOrigen = repository.Get<Planet>(planetId);
            //Player playerOrigen = repository.Get<Player>(x => x.Planets.Contains(planetOrigen));
            Player playerOrigen = repository.Get<Planet>(x => x.Id.Equals(planetId)).Conqueror;
            Planet planetDestino = repository.Get<Planet>(planetIdTarget);

            IList<Colonize> listaColonizadoresUsados = repository.List<Colonize>(x => x.ColonizerPlanet.Id.Equals(planetId));

            List<int> idColonizadoresUsadosList = (listaColonizadoresUsados != null) ? listaColonizadoresUsados.Select(x => x.Colonizer.Id).ToList() : new List<int>();

            IList<Probe> colonizadorList = repository.List<Probe>(x => x.Planet.Id.Equals(planetId) && x.EnableDate <= DateTime.Now && !idColonizadoresUsadosList.Contains(x.Id));

            if (colonizadorList != null && colonizadorList.Count > 0)
            {
                Probe colonizador = colonizadorList[0];
                repository.Add<Colonize>(new Colonize { ColonizerArrival = arrival, ColonizerDeparture = DateTime.Now, ColonizerPlanet = planetOrigen, ColonizerPlayer = playerOrigen, DestinationPlanet = planetDestino, Colonizer = colonizador });
                repository.SaveChanges();
            }
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

        public List<long> GetColonizesList(int playerId)
        {
            List<long> envios = new List<long>();

            IList<Colonize> colonizations = repository.List<Colonize>(x => x.ColonizerPlayer.Id.Equals(playerId) && x.ColonizerArrival > DateTime.Now);

            if (colonizations != null && colonizations.Count > 0)
            {
                foreach (var item in colonizations)
                {
                    long tiempo = this.GetMilli(item.ColonizerArrival);
                    envios.Add(tiempo);
                }
            }

            return (envios);
        }

        private long GetMilli(DateTime date)
        {
            return (Convert.ToInt64(date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds));
        }

        public void PerformColonize(Timer timer, int planetId, int planetIdTarget)
        {
            //destruyo el timer
            timer.Stop();
            timer.Dispose();

            //como el timer corre en otro thread no tengo el dbcontext instanciado
            using (var context = new GeolaxiaContext())
            {
                try
                {
                    var repo = new RepositoryService(context);

                    Planet planetaDestino = repo.Get<Planet>(x => x.Id.Equals(planetIdTarget));

                    if (planetaDestino != null && planetaDestino.Conqueror == null)
                    {
                        planetaDestino.Conqueror = repo.Get<Planet>(x => x.Id.Equals(planetId)).Conqueror;
                        
                        Colonize colonizacion = repo.Get<Colonize>(x => x.ColonizerPlanet.Id.Equals(planetId) && x.DestinationPlanet.Id.Equals(planetIdTarget));
                        
                        if (colonizacion != null)
                        {
                            colonizacion.MissionComplete = true;
                        }

                        repo.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }
    }
}
