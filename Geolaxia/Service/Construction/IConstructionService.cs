using Model;
using System.Collections.Generic;
using System.Timers;
using Model.DTO;
using Model.Enum;

namespace Service.Construction
{
    public interface IConstructionService
    {
        IList<Mine> GetCurrentMines(int planetId);

        IList<Mine> GetMinesToBuild(int planetId);

        Mine Add(Mine mine);

        Mine GetFromDTO(MineDTO mineDto);

        void FinishMine(Timer timer, MineDTO mineDto);

        EnergyCentral GetCurrentEnergyCentral(int planetId);

        EnergyFuelCentral GetCurrentEnergyFuelCentral(int planetId);

        IList<SolarPanel> GetCurrentSolarPanels(int planetId);

        IList<WindTurbine> GetCurrentWindTurbines(int planetId);

        IList<EnergyFacility> GetEnergyFacilitiesToBuild(int planetId);

        EnergyFacility Add(EnergyFacility energyFacility);

        void AddSolarPanels(int planetId, int qtt);

        void AddWindTurbines(int planetId, int qtt);

        EnergyFacility GetFromDTO(EnergyFacilityDTO energyFacilityDto);

        void FinishEnergyFacility(Timer timer, EnergyFacilityDTO energyFacilityDto);

        Hangar GetCurrentHangar(int planetId);

        Hangar AddHangar(int planetId);

        void FinishHangar(Timer timer, Hangar hangar);

        IList<Ship> GetCurrentShips(int planetId);

        void AddShips(int planetId, int qtt, ShipType shipType);

        void AddShipX(int planetId, int qtt);

        void AddShipY(int planetId, int qtt);

        void AddShipZ(int planetId, int qtt);

        IList<Probe> GetCurrentProbes(int planetId);

        IList<Trader> GetCurrentTraders(int planetId);

        Shield AddShield(int planetId);

        void AddProbes(int planetId, int qtt);

        void AddTraders(int planetId, int qtt);

        void FinishShield(Timer timer, Shield shield);

        void FinishProbe(Timer timer, Probe probe);

        void FinishTrader(Timer timer, Trader trader);
    }
}
