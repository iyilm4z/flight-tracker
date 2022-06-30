using System.Collections.Generic;

namespace FlightTracker.Csv.Read;

public interface IFlightTrackerCsvReader
{
    List<FlightCsvModel> ReadFlights();
    List<RouteCsvModel> ReadRoutes();
    List<SubscriptionCsvModel> ReadSubscriptions();
}