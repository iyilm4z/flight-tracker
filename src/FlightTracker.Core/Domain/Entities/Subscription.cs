namespace FlightTracker.Domain.Entities
{
    public class Subscription : IEntity
    {
        public int AgencyId { get; set; }

        public int OriginCityId { get; set; }

        public int DestinationCityId { get; set; }
    }
}