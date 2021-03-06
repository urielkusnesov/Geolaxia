﻿using Model.Enum;

namespace Model.DTO
{
    public class MineDTO
    {
        public int Id { get; set; }
        public int ConstructionTime { get; set; }
        public int CostId { get; set; }
        public int Level { get; set; }
        public int PlanetId { get; set; }
        public int Productivity { get; set; }
        public int EnergyConsumption { get; set; }
        public MineType MineType { get; set; }
    }
}
