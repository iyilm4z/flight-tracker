using System.Collections.Generic;
using FlightTracker.Application.Dto;

namespace FlightTracker.Csv.Export;

public interface IFlightTrackerExporter
{
    void ExportFlightsAsCsv(List<FlightDto> flights);
}