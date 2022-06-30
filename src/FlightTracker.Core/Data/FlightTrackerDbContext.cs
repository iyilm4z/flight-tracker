using FlightTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightTracker.Data
{
    public class FlightTrackerDbContext : DbContext, IFlightTrackerDbContext
    {
        public FlightTrackerDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureFlightTracker<FlightTrackerDbContext>();
        }

        // IFlightTrackerDbContext impl
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
    }
}