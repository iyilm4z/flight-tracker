using System.Collections.Generic;
using System.Linq;
using FlightTracker.Domain.Entities;

namespace FlightTracker.Application;

public static class FlightStatusFilterer
{
    public static List<Flight> FilterNewFlights(
        IReadOnlyDictionary<int, List<Flight>> currentFlightsByAirlineIdDictionary,
        IReadOnlyDictionary<int, List<Flight>> oldFlightsByAirlineIdDictionary)
    {
        var result = new List<Flight>();

        foreach (var currentFlightsByAirlineIdPair in currentFlightsByAirlineIdDictionary)
        {
            var oldFlightsByTheSameAirlineExists =
                oldFlightsByAirlineIdDictionary.TryGetValue(currentFlightsByAirlineIdPair.Key,
                    out var oldFlightsByAirlineId);

            if (!oldFlightsByTheSameAirlineExists)
            {
                result.AddRange(currentFlightsByAirlineIdPair.Value);

                continue;
            }

            foreach (var flight in currentFlightsByAirlineIdPair.Value)
            {
                var oneWeekAgo = flight.DepartureTime.AddDays(-7);
                var startDate = oneWeekAgo.AddMinutes(-30);
                var endDate = oneWeekAgo.AddMinutes(30);

                var oldFlightsByAirlineIdExists = oldFlightsByAirlineId.Any(
                    x => x.DepartureTime >= startDate
                         && x.DepartureTime <= endDate
                         && x.Route.OriginCityId == flight.Route.OriginCityId
                         && x.Route.DestinationCityId == flight.Route.DestinationCityId);

                if (oldFlightsByAirlineIdExists)
                {
                    continue;
                }

                result.Add(flight);
            }
        }

        return result;
    }

    public static List<Flight> FilterDiscontinuedFlights(
        IReadOnlyDictionary<int, List<Flight>> currentFlightsByAirlineIdDictionary,
        IReadOnlyDictionary<int, List<Flight>> oldFlightsByAirlineIdDictionary)
    {
        var result = new List<Flight>();

        foreach (var oldFlightsByAirlineIdPair in oldFlightsByAirlineIdDictionary)
        {
            var currentFlightsByTheSameAirlineExists =
                currentFlightsByAirlineIdDictionary.TryGetValue(oldFlightsByAirlineIdPair.Key,
                    out var currentFlightsByAirlineId);

            if (!currentFlightsByTheSameAirlineExists)
            {
                result.AddRange(oldFlightsByAirlineIdPair.Value);

                continue;
            }

            foreach (var flight in oldFlightsByAirlineIdPair.Value)
            {
                var oneWeekLater = flight.DepartureTime.AddDays(7);
                var startDate = oneWeekLater.AddMinutes(-30);
                var endDate = oneWeekLater.AddMinutes(30);

                var currentFlightsByAirlineIdExists = currentFlightsByAirlineId.Any(
                    x => x.DepartureTime >= startDate
                         && x.DepartureTime <= endDate
                         && x.Route.DestinationCityId == flight.Route.DestinationCityId
                         && x.Route.OriginCityId == flight.Route.OriginCityId);

                if (currentFlightsByAirlineIdExists)
                {
                    continue;
                }

                result.Add(flight);
            }
        }

        return result;
    }
}