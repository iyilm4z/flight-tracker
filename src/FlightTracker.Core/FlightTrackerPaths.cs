using System;
using System.IO;

namespace FlightTracker
{
    public static class FlightTrackerPaths
    {
        private static readonly string RootPath = Directory.GetCurrentDirectory();

        public static string Log => Path.Combine(RootPath, "flight-tracker.log");

        public static string SeedData => Path.Combine(RootPath, "App_Data", "seed");

        public static string ExportCsv => Path.Combine(RootPath, $"flight-tracker-{DateTime.Now.Ticks}.csv");
    }
}