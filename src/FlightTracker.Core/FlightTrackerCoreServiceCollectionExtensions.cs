using FlightTracker.Args;
using FlightTracker.Commands;
using FlightTracker.Commands.Detect;
using FlightTracker.Commands.Help;
using FlightTracker.Csv.Export;
using FlightTracker.Csv.Read;
using FlightTracker.Data;
using FlightTracker.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlightTracker
{
    public static class FlightTrackerCoreServiceCollectionExtensions
    {
        public static void RegisterFlightTrackerCoreServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            services.AddDbContext<FlightTrackerDbContext>(options => options.UseSqlite(connectionString));

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