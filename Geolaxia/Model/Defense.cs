using System;
using System.Collections.Generic;

namespace Model
{
    public class Defense
    {
        public virtual int Id { get; set; }
        public virtual Attack Attack { get; set; }
        public virtual Queztion Question1 { get; set; }
        public virtual Queztion Question2 { get; set; }
        public virtual Queztion Question3 { get; set; }
        //public virtual bool Question1Ok { get; set; }
        //public virtual bool Question2Ok { get; set; }
        //public virtual bool Question3Ok { get; set; }
        public virtual int Defended { get; set; }
    }
}
