﻿using System.Collections.Generic;
using Model;
using System;

namespace Service.Colonization
{
    public interface IColonizeService
    {
        IList<Colonizer> GetColonizers(int planetId);
        void SendColonizer(int planetId, int planetIdTarget, long time);
        long IsSendingColonizer(int planetId);
    }
}
