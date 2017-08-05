using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Attacks
{
    public class AttackService : IAttackService
    {
        private readonly IRepositoryService repository;

        public AttackService(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public Attack Get(int id)
        {
            return repository.Get<Attack>(id);
        }

        public Attack Add(Attack attack)
        {
            var result = repository.Add<Attack>(attack);
            repository.SaveChanges();
            return result;
        }

        public Attack Remove(Attack attack)
        {
            var result = repository.Remove<Attack>(attack);
            repository.SaveChanges();
            return result;
        }

        public Attack Remove(int id)
        {
            var result = repository.Remove<Attack>(id);
            repository.SaveChanges();
            return result;
        }
    }
}
