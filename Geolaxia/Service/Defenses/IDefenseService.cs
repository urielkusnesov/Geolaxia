using System.Collections.Generic;
using Model;

namespace Service.Defenses
{
    public interface IDefenseService
    {
        IList<Canon> GetCanons(int planetId);

        Shield GetShieldStatus(int planetId);

        void BuildCannons(int planetId, int cant);

        long IsBuildingCannons(int planetId);

        Shield GetCurrentShield(int planetId);

        List<Queztion> Get3RandomQuestions();

        int CreateDefense(int attackId, List<Queztion> questions);

        void DefenseFromAttack(int defenseId, int cantidadCorrectas);
    }
}
