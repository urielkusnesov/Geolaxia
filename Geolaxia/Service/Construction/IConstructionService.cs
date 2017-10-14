using Model;
using System.Collections.Generic;
using System.Timers;
using Model.DTO;

namespace Service.Construction
{
    public interface IConstructionService
    {
        IList<Mine> GetCurrentMines(int planetId);

        IList<Mine> GetMinesToBuild(int planetId);

        Mine Add(Mine mine);

        Mine GetFromDTO(MineDTO mineDto);

        void FinishMine(Timer timer, MineDTO mineDto);
    }
}
