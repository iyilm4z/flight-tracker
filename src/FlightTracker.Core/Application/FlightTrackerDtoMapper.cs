using System.Collections.Generic;
using System.Linq;
using FlightTracker.Application.Dto;
using FlightTracker.Domain.Entities;

namespace FlightTracker.Application;

internal static class FlightTrackerDtoMapper
{
    public static List<FlightDto> MapToFlightDto(this List<Flight> sourceFlights, List<Flight> allFlights)
    {
        var allFlightsByAirlineId = allFlights.GroupBy(x => x.AirlineId)
            .Select(grp => new
            {
                AirlineId = grp.Key,
                Flights = grp.ToList()
            })
            .ToList();

        var result = new List<FlightDto>();

        foreach (var sourceFlight in sourceFlights)
        {
            var flightsByAirlineId = allFlightsByAirlineId.FirstOrDefault(x => x.AirlineId == sourceFlight.AirlineId);

            var flightDto = new FlightDto
            {
                FlightId = sourceFlight.Id,
                OriginCityId = sourceFlight.Route.OriginCityId,
                DestinationCityId = sourceFlight.Route.DestinationCityId,
                DepartureTime = sourceFlight.DepartureTime,
                ArrivalTime = sourceFlight.ArrivalTime,
                AirlineId = sourceFlight.AirlineId
            };

            if (FlightStatusHelper.IsNewFlight(flightsByAirlineId.Flights, sourceFlight))
            {
                flightDto.Status = FlightStatus.New;
            }

            if (FlightStatusHelper.IsDiscontinuedFlight(flightsByAirlineId.Flights, sourceFlight))
            {
                flightDto.Status = FlightStatus.Discontinued;
            }

            result.Add(flightDto);
        }

        return result;
    }
}