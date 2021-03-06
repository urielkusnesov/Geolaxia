﻿using Model.Enum;

namespace Model
{
    public abstract class Ship : Military
    {
        public virtual int Attack { get; set; }
        public virtual int Defence { get; set; }
        public virtual int DarkMatterConsumption { get; set; }
        public virtual int Speed { get; set; }
        //para pasar le herencia al cliente
        public virtual ShipType ShipType { get; set; }
    }
}
