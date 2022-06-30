using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace FlightTracker.Csv.Read
{
    public class FlightTrackerCsvReader : IFlightTrackerCsvReader
    {
        public List<FlightCsvModel> ReadFlights()
        {
            var path = Path.Combine(FlightTrackerPaths.SeedData, "flights.csv");

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            return csv.GetRecords<FlightCsvModel>().ToList();
        }

        public List<RouteCsvModel> ReadRoutes()
        {
            var path = Path.Combine(FlightTrackerPaths.SeedData, "routes.csv");

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            return csv.GetRecords<RouteCsvModel>().ToList();
        }

        public List<SubscriptionCsvModel> ReadSubscriptions()
        {
            var path = Path.Combine(FlightTrackerPaths.SeedData, "subscriptions.csv");

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            return csv.GetRecords<SubscriptionCsvModel>().ToList();
        }
    }
}