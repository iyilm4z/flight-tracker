using FlightTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightTracker.Data
{
    public static class FlightTrackerDbContextModelBuilderExtensions
    {
        // ReSharper disable once UnusedTypeParameter
        public static void ConfigureFlightTracker<TDbContext>(this ModelBuilder builder)
            where TDbContext : IFlightTrackerDbContext
        {
            builder.ConfigureFlightEntity();
            builder.ConfigureRouteEntity();
            builder.ConfigureSubscriptionEntity();
        }

        private static void ConfigureFlightEntity(this ModelBuilder builder)
        {
            builder.Entity<Flight>(b =>
            {
                b.ToTable(nameof(Flight));
                b.HasKey(flight => flight.Id);
            });
        }

        private static void ConfigureRouteEntity(this ModelBuilder builder)
        {
            builder.Entity<Route>(b =>
            {
                b.ToTable(nameof(Route));
                b.HasKey(route => route.Id);
            });
        }

        private static void ConfigureSubscriptionEntity(this ModelBuilder builder)
        {
            builder.Entity<Subscription>(b =>
            {
                b.ToTable(nameof(Subscription));
                b.HasKey(subscription => new
                {
                    subscription.AgencyId,
                    subscription.OriginCityId,
                    subscription.DestinationCityId
                });
            });
        }
    }
}