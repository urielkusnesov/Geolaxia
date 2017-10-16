namespace Model
{
    public class Shield : Military
    {
        public Shield()
        {
            this.IsActive = false;
        }

        public virtual bool IsActive { get; set; }
    }
}
