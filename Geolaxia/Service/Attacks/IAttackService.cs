using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.DTO;

namespace Service.Attacks
{
    public interface IAttackService
    {
        Attack Get(int id);

        Attack Add(Attack attack);

        Attack Remove(Attack attack);

        Attack Remove(int id);

        Attack GetFromDTO(AttackDTO attackDTO);
    }
}
