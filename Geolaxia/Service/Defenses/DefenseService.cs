﻿using System;
using System.Collections.Generic;
using Model;
using Repository;

namespace Service.Defenses
{
    public class DefenseService : IDefenseService
    {
        private readonly IRepositoryService repository;

        private int CANON_CONST_TIEMPO = 3;
        private int CANON_CONST_COSTO_METAL = 100;
        private int CANON_CONST_COSTO_CRISTAL = 50;
        private int CANON_ATAQUE = 50;
        private int CANON_DEFENSA = 50;

        public DefenseService(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public IList<Canon> GetCanons(int planetId)
        {
            return repository.List<Canon>(x => x.Planet.Id == planetId && x.EnableDate < DateTime.Now);
        }

        public Shield GetShieldStatus(int planetId)
        {
            var shield = repository.Get<Shield>(x => x.Planet.Id == planetId && x.EnableDate < DateTime.Now);
            return shield;
        }

        public void BuildCannons(int planetId, int cant)
        {
            for (int i = 1; i <= cant; i++)
            {
                var result = repository.Add(new Canon { Planet = repository.Get<Planet>(planetId), Attack = this.CANON_ATAQUE, Defence = this.CANON_DEFENSA, EnableDate = DateTime.Now.AddMinutes(i * this.CANON_CONST_TIEMPO) });
            }

            var planet = repository.Get<Planet>(planetId);
            planet.Metal -= cant * this.CANON_CONST_COSTO_METAL;
            planet.Crystal -= cant * this.CANON_CONST_COSTO_CRISTAL;

            repository.SaveChanges();
        }

        public long IsBuildingCannons(int planetId)
        {
            long buildingTime = this.GetMilli(DateTime.MinValue);

            Canon cannon = repository.Max<Canon, DateTime>(x => x.Planet.Id.Equals(planetId) && x.EnableDate > DateTime.Now, x => x.EnableDate);

            if (cannon != null)
            {
                buildingTime = this.GetMilli(cannon.EnableDate);
            }

            return (buildingTime);
        }

        private long GetMilli(DateTime date)
        {
            return (Convert.ToInt64(date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds));
        }

        public Shield GetCurrentShield(int planetId)
        {
            var shield = repository.Get<Shield>(x => x.Planet.Id == planetId);

            if (shield == null)
            {
                var shieldCost = repository.Get<Cost>(x => x.Element == "shield");
                shield = new Shield{Cost = shieldCost, Planet = new BlackPlanet(), ConstructionTime = 120, RequiredLevel = 5};
            }
            return shield;
        }
    }
}
