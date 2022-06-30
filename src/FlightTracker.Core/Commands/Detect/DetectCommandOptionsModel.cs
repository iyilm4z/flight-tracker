using System;

namespace FlightTracker.Commands.Detect
{
    public class DetectCommandOptionsModel
    {
        public DetectCommandOptionsModel(DateTime startDate, DateTime endDate, int agencyId)
        {
            StartDate = startDate;
            EndDate = endDate;
            AgencyId = agencyId;
        }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public int AgencyId { get; }
    }
}