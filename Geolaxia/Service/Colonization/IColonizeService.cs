using System.Collections.Generic;
using Model;
using System;
using System.Timers;

namespace Service.Colonization
{
    public interface IColonizeService
    {
        IList<Probe> GetColonizers(int planetId);
        void SendColonizer(int planetId, int planetIdTarget, long time);
        long IsSendingColonizer(int planetId);
        List<long> GetColonizesList(int playerId);
        void PerformColonize(Timer timer, int planetId, int planetIdTarget);
    }
}
