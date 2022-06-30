using System;
using System.Threading.Tasks;
using FlightTracker.Data;
using FlightTracker.Data.Seed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace FlightTracker
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            ConfigureSerilog();

            try
            {
                var host = CreateHostBuilder(args).Build();

                ServiceProviderHelper.Instance.Services = host.Services;

                await host.SeedDatabase().RunAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureSerilog()
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Override("FlightTracker", LogEventLevel.Debug)
#else
            .MinimumLevel.Override("FlightTracker", LogEventLevel.Information)
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File(FlightTrackerPaths.Log))
                .WriteTo.Async(c => c.Console())
                .CreateLogger();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
                    services.RegisterFlightTrackerCoreServices(hostContext.Configuration);
                    services.AddHostedService<FlightTrackerHostedService>();
                })
                .UseSerilog()
                .UseConsoleLifetime();

            return host;
        }

        private static IHost SeedDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<FlightTrackerDbContext>();

            var seeder = new FlightTrackerSeeder(dbContext);
            seeder.Seed();

            return host;
        }
    }
}