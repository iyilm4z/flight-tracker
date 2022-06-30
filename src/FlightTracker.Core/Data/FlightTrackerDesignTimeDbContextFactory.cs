using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FlightTracker.Data
{
    // ReSharper disable once UnusedType.Global
    public class FlightTrackerDesignTimeDbContextFactory : IDesignTimeDbContextFactory<FlightTrackerDbContext>
    {
        public FlightTrackerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FlightTrackerDbContext>();
            optionsBuilder.UseSqlite("Filename=App_Data\\db\\flight-tracker.db");

            return new FlightTrackerDbContext(optionsBuilder.Options);
        }
    }
}