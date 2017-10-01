using Model;
using System.Collections.Generic;

namespace Service.Construction
{
    public interface IConstructionService
    {
        IList<Mine> GetCurrentMines(int planetId);

        IList<Mine> GetMinesToBuild(int planetId);

        Mine Add(Mine mine);

    }
}
