using System.Collections.Generic;
using Model;
using System;

namespace Service.Defenses
{
    public interface IDefenseService
    {
        IList<Canon> GetCanons(int planetId);
        Shield GetShieldStatus(int planetId);
        void BuildCannons(int planetId, int cant);
        long IsBuildingCannons(int planetId);
    }
}
