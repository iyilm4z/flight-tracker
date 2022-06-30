using System.Collections.Generic;
using System.Linq;
using FlightTracker.Application.Dto;
using FlightTracker.Domain.Entities;

namespace FlightTracker.Application;

internal static class FlightTrackerDtoMapper
{
    public static List<FlightDto> MapToFlightDto(this IEnumerable<Flight> sourceFlights, IEnumerable<Flight> oldFlights)
    {
        var sourceFlightsByAirlineIdDictionary = sourceFlights
            .GroupBy(x => x.AirlineId)
            .ToDictionary(grp => grp.Key, grp => grp.ToList());

        var oldFlightsByAirlineIdDictionary = oldFlights
            .GroupBy(x => x.AirlineId)
            .ToDictionary(grp => grp.Key, grp => grp.ToList());

        var result = new List<FlightDto>();

        BindNewFlights(result, sourceFlightsByAirlineIdDictionary, oldFlightsByAirlineIdDictionary);

        BindDiscontinuedFlights(result, sourceFlightsByAirlineIdDictionary, oldFlightsByAirlineIdDictionary);

        return result;
    }

    private static void BindNewFlights(List<FlightDto> result,
        IReadOnlyDictionary<int, List<Flight>> sourceFlightsByAirlineIdDictionary,
        IReadOnlyDictionary<int, List<Flight>> oldFlightsByAirlineIdDictionary)
    {
        var newFlights =
            FlightStatusFilterer.FilterNewFlights(sourceFlightsByAirlineIdDictionary,
                oldFlightsByAirlineIdDictionary);

        var flightDtos = newFlights.Select(flight => new FlightDto
        {
            FlightId = flight.Id,
            OriginCityId = flight.Route.OriginCityId,
            DestinationCityId = flight.Route.DestinationCityId,
            AirlineId = flight.AirlineId,
            Status = FlightStatus.New,
            DepartureTime = flight.DepartureTime,
            ArrivalTime = flight.ArrivalTime
        });

        result.AddRange(flightDtos);
    }

    private static void BindDiscontinuedFlights(List<FlightDto> result,
        IReadOnlyDictionary<int, List<Flight>> sourceFlightsByAirlineIdDictionary,
        IReadOnlyDictionary<int, List<Flight>> oldFlightsByAirlineIdDictionary)
    {
        var discontinuedFlights =
            FlightStatusFilterer.FilterDiscontinuedFlights(sourceFlightsByAirlineIdDictionary,
                oldFlightsByAirlineIdDictionary);

        var flightDtos = discontinuedFlights.Select(flight => new FlightDto
        {
            FlightId = flight.Id,
            OriginCityId = flight.Route.OriginCityId,
            DestinationCityId = flight.Route.DestinationCityId,
            AirlineId = flight.AirlineId,
            Status = FlightStatus.Discontinued,
            // for showing an old flight in matching flights
            DepartureTime = flight.DepartureTime.AddDays(7),
            ArrivalTime = flight.ArrivalTime.AddDays(7)
        });

        result.AddRange(flightDtos);
    }
}