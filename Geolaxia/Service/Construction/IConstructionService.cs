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
    }
}
