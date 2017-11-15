namespace Model
{
    public class Trader : Ship
    {
        public virtual int MaxCrystal { get; set; }
        public virtual int MaxMetal { get; set; }
        public virtual int MaxDarkMatter { get; set; }
        public Planet Target { get; set; }
        public Cost Load { get; set; }
    }
}
