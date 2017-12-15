using System.Collections.Generic;
using Model;
using System.Timers;

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

        //int CreateDefense(int attackId, List<Queztion> questions);

        //void DefenseFromAttack(int defenseId, int cantidadCorrectas);

        //long IsUnderAttack(int planetId);

        void DefenseFromAttack(int attackId, int idPreg1, int idPreg2, int idPreg3, int cantidadCorrectas);

        int ObtenerIdAtaqueMasProximo(int planetId);

        int ObtenerIdAtaqueMasProximoNoDefendido(int planetId);

        void MandarPushCanonTerminado(Timer timer, int planetId, int cant);
    }
}
