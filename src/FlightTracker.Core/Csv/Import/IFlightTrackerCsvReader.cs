using System.Collections.Generic;

namespace FlightTracker.Csv.Import;

public interface IFlightTrackerCsvReader
{
    List<FlightCsvModel> ReadFlights();
    List<RouteCsvModel> ReadRoutes();
    List<SubscriptionCsvModel> ReadSubscriptions();
}