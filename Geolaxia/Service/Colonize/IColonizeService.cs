using System.Collections.Generic;
using Model;
using System;

namespace Service.Colonize
{
    public interface IColonizeService
    {
        IList<Colonizer> GetColonizers(int planetId);
        //long IsBuildingCannons(int planetId);
    }
}
