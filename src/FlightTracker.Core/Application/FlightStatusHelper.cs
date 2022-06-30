using System.Collections.Generic;
using System.Linq;
using FlightTracker.Domain.Entities;

namespace FlightTracker.Application;

public static class FlightStatusHelper
{
    /// <summary>
    /// A flight with departure time T is considered to be a new flight
    /// if no corresponding flight from same airline exists with departure time T’ = T - 7 days (+/- 30 minutes tolerance).
    /// </summary>
    /// <param name="flights">all flights from the same airline</param>
    /// <param name="flight">a flight from the same airline</param>
    /// <returns>true/false</returns>
    public static bool IsNewFlight(IEnumerable<Flight> flights, Flight flight)
    {
        var sevenDaysAgo = flight.DepartureTime.AddDays(-7);
        var startDate = sevenDaysAgo.AddMinutes(-30);
        var endDate = sevenDaysAgo.AddMinutes(30);

        return !flights.Any(x => x.DepartureTime >= startDate && x.DepartureTime <= endDate);
    }

    /// <summary>
    /// A flight with departure time T is considered to be discontinued if no corresponding flight from same airline
    /// exists with departure time T’ = T + 7 days (+/- 30 minutes tolerance).
    /// </summary>
    /// <param name="flights">all flights from an airline</param>
    /// <param name="flight">a flight from the same airline</param>
    /// <returns>true/false</returns>
    public static bool IsDiscontinuedFlight(IEnumerable<Flight> flights, Flight flight)
    {
        var sevenDaysLater = flight.DepartureTime.AddDays(7);
        var startDate = sevenDaysLater.AddMinutes(-30);
        var endDate = sevenDaysLater.AddMinutes(30);

        return !flights.Any(x => x.DepartureTime >= startDate && x.DepartureTime <= endDate);
    }
}