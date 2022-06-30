namespace FlightTracker.Commands.Detect
{
    public static class DetectCommandOptionsOptions
    {
        public static class DepartureTime
        {
            public const string Short = "dt";
            public const string Long = "departure-time";
        }

        public static class ArrivalTime
        {
            public const string Short = "at";
            public const string Long = "arrival-time";
        }

        public static class AgencyId
        {
            public const string Short = "ai";
            public const string Long = "agency-id";
        }
    }
}