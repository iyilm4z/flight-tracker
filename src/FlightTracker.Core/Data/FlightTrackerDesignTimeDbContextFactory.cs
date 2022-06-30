using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FlightTracker.Data
{
    // ReSharper disable once UnusedType.Global
    public class FlightTrackerDesignTimeDbContextFactory : IDesignTimeDbContextFactory<FlightTrackerDbContext>
    {
        public FlightTrackerDbContext CreateDbContext(string[] args)
        {
            var configuration = GetConfiguration();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<FlightTrackerDbContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new FlightTrackerDbContext(optionsBuilder.Options);
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(FlightTrackerPaths.RootPath)
                .AddJsonFile("appsettings.json");

            return builder.Build();
        }
    }
}