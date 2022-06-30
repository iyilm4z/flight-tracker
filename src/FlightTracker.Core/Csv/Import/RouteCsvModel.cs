using System;
using CsvHelper.Configuration.Attributes;

namespace FlightTracker.Csv.Import
{
    public class RouteCsvModel
    {
        public const string OriginCityIdName = "origin_city_id";
        public const string DestinationCityIdName = "destination_city_id";
        
        [Name("route_id")] public int RouteId { get; set; }

        [Name(OriginCityIdName)] public int OriginCityId { get; set; }

        [Name(DestinationCityIdName)] public int DestinationCityId { get; set; }

        [Name("departure_date")] public DateTime DepartureDate { get; set; }
    }
}