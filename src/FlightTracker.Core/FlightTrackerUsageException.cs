using System;

namespace FlightTracker
{
    public class FlightTrackerUsageException : Exception
    {
        public FlightTrackerUsageException(string message)
            : base(message)
        {

        }

        public FlightTrackerUsageException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
