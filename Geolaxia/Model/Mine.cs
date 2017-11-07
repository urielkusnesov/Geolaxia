using Model.Enum;
namespace Model
{
    public abstract class Mine : Facility
    {
        public virtual int EnergyConsumption { get; set; }
        public virtual MineType MineType { get; set; }

        public abstract void AddResource(Planet planet);
    }
}
