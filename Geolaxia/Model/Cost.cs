namespace Model
{
    public class Cost
    {
        public virtual int Id { get; set; }
        public virtual int CrystalCost { get; set; }
        public virtual int MetalCost { get; set; }
        public virtual int DarkMatterCost { get; set; }
    }
}
