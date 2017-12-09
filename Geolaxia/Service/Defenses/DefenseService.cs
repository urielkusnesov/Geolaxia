using System;
using System.Collections.Generic;
using Model;
using Repository;
using System.Linq;

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
        private int PREGUNTAS_CANTIDAD_A_RESPONDER = 3;
        private int PREGUNTAS_CANTIDAD_EN_BASE = 24; //tiene que ser la (cantidad + 1) por el random
        private int TIEMPO_AVISO_PARA_DEFENSA = 3600; //en segundos

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
                shield = new Shield
                {
                    Cost = shieldCost,
                    Planet = new BlackPlanet(),
                    ConstructionTime = 120,
                    RequiredLevel = 5
                };
            }
            return shield;
        }

        public List<Queztion> Get3RandomQuestions()
        {
            List<Queztion> questions = new List<Queztion>();
            Queztion question = null;
            Random rnd = new Random();
            int id = 0;

            while (questions.Count != this.PREGUNTAS_CANTIDAD_A_RESPONDER)
            {
                id = rnd.Next(1, this.PREGUNTAS_CANTIDAD_EN_BASE);

                question = repository.Get<Queztion>(x => x.Id.Equals(id));

                if (!questions.Exists(x => x.Id.Equals(question.Id)))
                {
                    questions.Add(question);
                }
            }

            return (questions);
        }

        //public int CreateDefense(int attackId, List<Queztion> questions)
        //{
        //    Attack attack = repository.Get<Attack>(x => x.Id.Equals(attackId));
        //    Queztion question1 = repository.Get<Queztion>(x => x.Id.Equals(questions[0].Id));
        //    Queztion question2 = repository.Get<Queztion>(x => x.Id.Equals(questions[1].Id));
        //    Queztion question3 = repository.Get<Queztion>(x => x.Id.Equals(questions[2].Id));

        //    Defense defense = repository.Add<Defense>(new Defense { Attack = attack, Question1 = question1, Question2 = question2, Question3 = question3 });

        //    repository.SaveChanges();

        //    return (defense.Id);
        //}

        //public void DefenseFromAttack(int defenseId, int cantidadCorrectas)
        //{
        //    Defense defense = repository.Get<Defense>(x => x.Id.Equals(defenseId));

        //    if (cantidadCorrectas.Equals(3)) 
        //    {
        //        defense.Defended = 50;
        //    }
        //    else if (cantidadCorrectas.Equals(2))
        //    {
        //        defense.Defended = 30;
        //    }
        //    else if (cantidadCorrectas.Equals(1))
        //    {
        //        defense.Defended = 10;
        //    }
        //    else
        //    {
        //        defense.Defended = 0;
        //    }

        //    repository.SaveChanges();
        //}

        private int CreateDefense(int attackId, int idPreg1, int idPreg2, int idPreg3)
        {
            Attack attack = repository.Get<Attack>(x => x.Id.Equals(attackId));
            Queztion question1 = repository.Get<Queztion>(x => x.Id.Equals(idPreg1));
            Queztion question2 = repository.Get<Queztion>(x => x.Id.Equals(idPreg2));
            Queztion question3 = repository.Get<Queztion>(x => x.Id.Equals(idPreg3));

            Defense defense = repository.Add<Defense>(new Defense { Attack = attack, Question1 = question1, Question2 = question2, Question3 = question3 });

            repository.SaveChanges();

            return (defense.Id);
        }

        public void DefenseFromAttack(int attackId, int idPreg1, int idPreg2, int idPreg3, int cantidadCorrectas)
        {
            int defenseId = this.CreateDefense(attackId, idPreg1, idPreg2, idPreg3);

            Defense defense = repository.Get<Defense>(x => x.Id.Equals(defenseId));

            if (cantidadCorrectas.Equals(3))
            {
                defense.Defended = 50;
            }
            else if (cantidadCorrectas.Equals(2))
            {
                defense.Defended = 30;
            }
            else if (cantidadCorrectas.Equals(1))
            {
                defense.Defended = 10;
            }
            else
            {
                defense.Defended = 0;
            }

            repository.SaveChanges();
        }

        private Attack ObtenerAtaqueMasProximo(int planetId)
        {
            DateTime intervalo = DateTime.Now.AddSeconds(this.TIEMPO_AVISO_PARA_DEFENSA);

            Attack attack = repository.Min<Attack, DateTime>(x => x.DestinationPlanet.Id.Equals(planetId) && x.FleetArrival > DateTime.Now && x.FleetArrival < intervalo, x => x.FleetArrival);

            if (attack != null)
            {
                return (attack);
            }

            return(null);
        }

        private Attack ObtenerAtaqueMasProximoNoDefendido(int planetId)
        {
            DateTime intervalo = DateTime.Now.AddSeconds(this.TIEMPO_AVISO_PARA_DEFENSA);

            //Attack attack = repository.Min<Attack, DateTime>(x => x.DestinationPlanet.Id.Equals(planetId) && x.FleetArrival > DateTime.Now && x.FleetArrival < intervalo, x => x.FleetArrival);
            IList<Attack> attackList = repository.List<Attack>(x => x.DestinationPlanet.Id.Equals(planetId) && x.FleetArrival > DateTime.Now && x.FleetArrival < intervalo).OrderBy(x => x.FleetArrival).ToList();

            foreach (var attack in attackList)
            {
                if (attack != null)
                {
                    Defense defense = repository.Get<Defense>(x => x.Attack.Id.Equals(attack.Id));

                    if (defense == null)
                    {
                        return (attack);
                    }
                }
            }

            return (null);
        }

        public int ObtenerIdAtaqueMasProximo(int planetId)
        {
            Attack attack = this.ObtenerAtaqueMasProximo(planetId);

            if (attack != null)
            {
                return (attack.Id);
            }

            return (0);
        }

        public int ObtenerIdAtaqueMasProximoNoDefendido(int planetId)
        {
            Attack attack = this.ObtenerAtaqueMasProximoNoDefendido(planetId);

            if (attack != null)
            {
                return (attack.Id);
            }

            return (0);
        }
    }
}
