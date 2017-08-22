namespace Model
{
    public abstract class Military
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int ConstructionTime { get; set; }
        public virtual Cost Cost { get; set; }
        public virtual Planet Planet { get; set; }
        public virtual int RequiredLevel { get; set; }
    }
}
