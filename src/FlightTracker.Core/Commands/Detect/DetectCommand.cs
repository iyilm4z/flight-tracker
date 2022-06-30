using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightTracker.Application;
using FlightTracker.Args;
using FlightTracker.Csv.Export;
using FlightTracker.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightTracker.Commands.Detect
{
    public class DetectCommand : IConsoleCommand
    {
        public const string Name = "detect";

        private readonly ILogger<DetectCommand> _logger;
        private readonly FlightRepository _flightRepository;
        private readonly IFlightTrackerExporter _flightTrackerExporter;

        public DetectCommand(ILogger<DetectCommand> logger,
            FlightRepository flightRepository,
            IFlightTrackerExporter flightTrackerExporter)
        {
            _logger = logger;
            _flightRepository = flightRepository;
            _flightTrackerExporter = flightTrackerExporter;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            _logger.LogInformation("Detecting flights started");

            var optionsModel = GetValidatedOptionsModel(commandLineArgs.Options);

            var flights = await _flightRepository
                .GetFlightsAsync(optionsModel.DepartureTime, optionsModel.ArrivalTime, optionsModel.AgencyId);

            var flightsByAirlineIds = flights.Select(x => x.AirlineId).Distinct().ToArray();

            var allFlights = await _flightRepository
                .GetFlightsAsync(optionsModel.DepartureTime, optionsModel.ArrivalTime, flightsByAirlineIds);

            var flightDtos = flights.MapToFlightDto(allFlights);

            _flightTrackerExporter.ExportFlightsAsCsv(flightDtos);

            _logger.LogInformation(!flightDtos.Any()
                ? "No flight found"
                : $"{flightDtos.Count} flight(s) found and exported as csv");

            stopwatch.Stop();

            _logger.LogInformation($"Time taken: {stopwatch.Elapsed}");
        }

        private DetectCommandOptionsModel GetValidatedOptionsModel(CommandLineOptions options)
        {
            var departureTimeOption = options.GetOrNull(DetectCommandOptionsOptions.DepartureTime.Short,
                DetectCommandOptionsOptions.DepartureTime.Long);

            if (string.IsNullOrEmpty(departureTimeOption) ||
                !DateTime.TryParse(departureTimeOption, out var departureTime))
            {
                throw new FlightTrackerUsageException(
                    "<departure-time> is missing or invalid!" +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }

            var arrivalTimeOption = options.GetOrNull(DetectCommandOptionsOptions.ArrivalTime.Short,
                DetectCommandOptionsOptions.ArrivalTime.Long);

            if (string.IsNullOrEmpty(arrivalTimeOption) ||
                !DateTime.TryParse(arrivalTimeOption, out var arrivalTime))
            {
                throw new FlightTrackerUsageException(
                    "<arrival-time> is missing or invalid!" +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }

            var agencyIdOption = options.GetOrNull(DetectCommandOptionsOptions.AgencyId.Short,
                DetectCommandOptionsOptions.AgencyId.Long);

            if (string.IsNullOrEmpty(agencyIdOption) ||
                !int.TryParse(agencyIdOption, out var agencyId))
            {
                throw new FlightTrackerUsageException(
                    "<agency-id> is missing or invalid!" +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }

            return new DetectCommandOptionsModel(departureTime, arrivalTime, agencyId);
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine($"  flight-tracker {Name} -dt <departure-time> -at <arrival-time> -ai <agency-id>");
            sb.AppendLine("");
            sb.AppendLine("Example:");
            sb.AppendLine("");
            sb.AppendLine($"  flight-tracker {Name} -dt 2018-01-01 -at 2018-01-15 -ai 1");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Detect flight schedule changes in airline competitive landscape.";
        }
    }
}