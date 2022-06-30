using System.Collections.Generic;
using System.IO;
using System.Text;
using FlightTracker.Application.Dto;
using FlightTracker.Csv.Import;

namespace FlightTracker.Csv.Export;

public class FlightTrackerExporter : IFlightTrackerExporter
{
    public void ExportFlightsAsCsv(List<FlightDto> flights)
    {
        using var stream = File.AppendText(FlightTrackerPaths.ExportCsv);

        var sb = new StringBuilder();

        // write headers
        sb.AppendLine($"{FlightCsvModel.FlightIdName},{RouteCsvModel.OriginCityIdName}," +
                      $"{RouteCsvModel.DestinationCityIdName},{FlightCsvModel.DepartureTimeName}," +
                      $"{FlightCsvModel.ArrivalTimeName},{FlightCsvModel.AirlineIdName},status");

        foreach (var flight in flights)
        {
            sb.AppendLine($"{flight.FlightId},{flight.OriginCityId}," +
                          $"{flight.DestinationCityId},{flight.DepartureTime}," +
                          $"{flight.ArrivalTime},{flight.AirlineId},{flight.Status}");
        }

        stream.WriteLine(sb.ToString());
    }
}