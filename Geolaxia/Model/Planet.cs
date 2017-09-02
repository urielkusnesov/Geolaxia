﻿using Model.Enum;

namespace Model
{
    public abstract class Planet
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Order { get; set; }
        public virtual Player Conqueror { get; set; }
        public virtual int Crystal { get; set; }
        public virtual int Metal { get; set; }
        public virtual int DarkMatter { get; set; }
        public virtual int Energy { get; set; }
        public virtual bool IsOrigin { get; set; }
        public virtual SolarSystem SolarSystem { get; set; }
        public virtual int PositionX { get; set; }
        public virtual int PositionY { get; set; }
        public virtual int PositionZ { get; set; }
        //para pasar le herencia al cliente
        public virtual PlanetType PlanetType { get; set; }
    }
}
