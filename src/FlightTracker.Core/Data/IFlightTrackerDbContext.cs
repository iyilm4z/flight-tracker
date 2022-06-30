using FlightTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightTracker.Data
{
    public interface IFlightTrackerDbContext
    {
        DbSet<Flight> Flights { get; set; }
        DbSet<Route> Routes { get; set; }
        DbSet<Subscription> Subscriptions { get; set; }
    }
}