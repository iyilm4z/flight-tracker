using System;

namespace FlightTracker.Domain.Entities
{
    public class Flight : EntityBase
    {
        public int RouteId { get; set; }

        public int AirlineId { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public Route Route { get; set; }
    }
}