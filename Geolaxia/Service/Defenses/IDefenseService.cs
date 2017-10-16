using System.Collections.Generic;
using Model;

namespace Service.Defenses
{
    public interface IDefenseService
    {
        IList<Canon> GetCanons(int planetId);
        Shield GetShieldStatus(int planetId);
    }
}
