namespace FlightTracker.Domain.Entities
{
    public abstract class EntityBase : EntityBase<int>
    {
    }

    public abstract class EntityBase<TId>
    {
        public TId Id { get; set; }
    }
}