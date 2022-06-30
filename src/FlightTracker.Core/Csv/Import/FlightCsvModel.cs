using System;
using CsvHelper.Configuration.Attributes;

namespace FlightTracker.Csv.Import
{
    public class FlightCsvModel
    {
        public const string FlightIdName = "flight_id";
        public const string DepartureTimeName = "departure_time";
        public const string ArrivalTimeName = "arrival_time";
        public const string AirlineIdName = "airline_id";

        [Name(FlightIdName)] public int FlightId { get; set; }

        [Name("route_id")] public int RouteId { get; set; }


        [Name(DepartureTimeName)] public DateTime DepartureTime { get; set; }


        [Name(ArrivalTimeName)] public DateTime ArrivalTime { get; set; }

        [Name(AirlineIdName)] public int AirlineId { get; set; }
    }
}