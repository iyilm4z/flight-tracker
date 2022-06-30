using FlightTracker.Args;
using FlightTracker.Commands;
using FlightTracker.Commands.Detect;
using FlightTracker.Commands.Help;
using FlightTracker.Csv.Export;
using FlightTracker.Csv.Import;
using FlightTracker.Data;
using FlightTracker.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlightTracker
{
    public static class FtCoreServiceCollectionExtensions
    {
        public static void RegisterFlightTrackerCoreServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<FlightTrackerDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<FlightTrackerService>();
            services.AddTransient<ICommandSelector, CommandSelector>();
            services.AddTransient<ICommandLineArgumentParser, CommandLineArgumentParser>();
            services.AddTransient<HelpCommand>();
            services.AddTransient<DetectCommand>();
            var cliOptions = new FlightTrackerSettings
            {
                Commands =
                {
                    [HelpCommand.Name] = typeof(HelpCommand),
                    [DetectCommand.Name] = typeof(DetectCommand)
                }
            };

            services.AddSingleton(cliOptions);

            services.AddTransient<FlightRepository>();
            services.AddTransient<IFlightTrackerExporter, FlightTrackerExporter>();
            services.AddTransient<IFlightTrackerCsvReader, FlightTrackerCsvReader>();
        }
    }
}