using System;
using System.Collections.Generic;

namespace FlightTracker
{
    public class FlightTrackerSettings
    {
        public Dictionary<string, Type> Commands { get; }

        public FlightTrackerSettings()
        {
            Commands = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        }
    }
}