using Model;
using System.Collections.Generic;

namespace Service.Ships
{
    public interface IShipService
    {
        Ship Get(int id);

        IList<Ship> GetByPlanet(int planetId);
    }
}
