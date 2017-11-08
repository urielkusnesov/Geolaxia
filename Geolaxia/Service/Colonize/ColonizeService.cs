using System;
using System.Collections.Generic;
using Model;
using Repository;

namespace Service.Colonize
{
    public class ColonizeService : IColonizeService
    {
        private readonly IRepositoryService repository;

        private int CANON_CONST_TIEMPO = 3;
        private int CANON_CONST_COSTO_METAL = 100;
        private int CANON_CONST_COSTO_CRISTAL = 50;
        private int CANON_ATAQUE = 50;
        private int CANON_DEFENSA = 50;
        private int PREGUNTAS_CANTIDAD_A_RESPONDER = 3;
        private int PREGUNTAS_CANTIDAD_EN_BASE = 24; //tiene que ser la (cantidad + 1) por el random

        public ColonizeService(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public IList<Colonizer> GetColonizers(int planetId)
        {
            return repository.List<Colonizer>(x => x.Planet.Id == planetId && x.EnableDate < DateTime.Now);
        }

        //public long IsBuildingCannons(int planetId)
        //{
        //    long buildingTime = this.GetMilli(DateTime.MinValue);

        //    Canon cannon = repository.Max<Canon, DateTime>(x => x.Planet.Id.Equals(planetId) && x.EnableDate > DateTime.Now, x => x.EnableDate);

        //    if (cannon != null)
        //    {
        //        buildingTime = this.GetMilli(cannon.EnableDate);
        //    }

        //    return (buildingTime);
        //}
    }
}
