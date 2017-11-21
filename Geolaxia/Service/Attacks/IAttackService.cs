using Model;
using Model.DTO;
using System.Collections.Generic;
using System.Timers;

namespace Service.Attacks
{
    public interface IAttackService
    {
        Attack Get(int id);

        Attack Add(Attack attack);

        Attack Remove(Attack attack);

        Attack Remove(int id);

        Attack GetFromDTO(AttackDTO attackDTO);

        void PerformAttack(Timer timer, Attack attack);

        //List<long> GetAttacksList(int playerId);
        IList<Attack> GetAttacksList(int playerId);

        //List<long> GetDefensesList(int playerId);
        IList<Attack> GetDefensesList(int playerId);
    }
}
