using Model;
using System.Collections.Generic;
using System.Timers;

namespace Service.Ships
{
    public interface IShipService
    {
        Ship Get(int id);

        IList<Ship> GetByPlanet(int planetId);

        IList<Ship> GetShipsCost();

        void FinishShip(Timer timer, Ship ship);
    }
}
