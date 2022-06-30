using System;

namespace FlightTracker.Domain.Entities
{
    public class Route : EntityBase
    {
        public int OriginCityId { get; set; }

        public int DestinationCityId { get; set; }

        public DateTime DepartureDate { get; set; }
    }
}