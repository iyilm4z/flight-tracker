using System;

namespace FlightTracker.Commands.Detect
{
    public class DetectCommandOptionsModel
    {
        public DetectCommandOptionsModel(DateTime departureTime, DateTime arrivalTime, int agencyId)
        {
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            AgencyId = agencyId;
        }

        public DateTime DepartureTime { get; }

        public DateTime ArrivalTime { get; }

        public int AgencyId { get; }
    }
}