namespace Model
{
    public class WindTurbine : Facility
    {
        public virtual int Threshold { get; set; }
        public virtual int BestProd { get; set; }
        public virtual int WorstProd { get; set; }
    }
}
