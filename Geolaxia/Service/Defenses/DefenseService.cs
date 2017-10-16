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
            Shield shield = repository.Get<Shield>(x => x.Planet.Id == planetId && x.EnableDate < DateTime.Now);

            return (shield);
        }

        public void BuildCannons(int planetId, int cant)
        {
            for (int i = 1; i <= cant; i++)
            {
                var result = repository.Add(new Canon { Planet = repository.Get<Planet>(planetId) , Attack = this.CANON_ATAQUE, Defence = this.CANON_DEFENSA, EnableDate = DateTime.Now.AddMinutes(i * this.CANON_CONST_TIEMPO)});
            }

            var planet = repository.Get<Planet>(planetId);
            planet.Metal -= cant * this.CANON_CONST_COSTO_METAL;
            planet.Crystal -= cant * this.CANON_CONST_COSTO_CRISTAL;

            repository.SaveChanges();
        }
    }
}
