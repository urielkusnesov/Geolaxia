using System;

namespace Model
{
    public abstract class Facility
    {
        public virtual int Id { get; set; }
        public virtual int ConstructionTime { get; set; }
        public virtual Cost Cost { get; set; }
        public virtual int Level { get; set; }
        public virtual Planet Planet { get; set; }
        public virtual int Productivity { get; set; }
        public virtual DateTime EnableDate { get; set; }
    }
}
