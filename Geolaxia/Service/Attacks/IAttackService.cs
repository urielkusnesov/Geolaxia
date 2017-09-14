using Model;
using Model.DTO;
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
    }
}
