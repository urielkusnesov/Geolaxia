﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;

    public partial class Player
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual int Level { get; set; }
        public virtual int ResourcesUsed { get; set; }
        public virtual IList<Planet> Planets { get; set; }
        public virtual string Token { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string FacebookId { get; set; }
        public virtual string lastLatitude { get; set; }
        public virtual string lastLongitude { get; set; }
    }
}
