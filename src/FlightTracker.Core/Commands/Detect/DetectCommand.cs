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

            var oneWeekAgoFromStartDate = optionsModel.StartDate.AddDays(-7);

            // All flights between one week ago from StartDate and EndDate
            var allFlights = await _flightRepository
                .GetFlightsAsync(oneWeekAgoFromStartDate, optionsModel.EndDate, optionsModel.AgencyId);

            // The flights until StartDate
            var oldFlights = allFlights.Where(x => x.DepartureTime < optionsModel.StartDate).ToList();

            // The flights matching between StartDate and EndDate criteria
            var matchingFlights = allFlights.Where(x => x.DepartureTime >= optionsModel.StartDate).ToList();

            var flightDtos = matchingFlights.MapToFlightDto(oldFlights);

            _flightTrackerExporter.ExportFlightsAsCsv(flightDtos);

            _logger.LogInformation(!flightDtos.Any()
                ? "No flight found"
                : $"{flightDtos.Count} flight(s) found and exported as csv");

            stopwatch.Stop();

            _logger.LogInformation($"Time taken: {stopwatch.Elapsed}");
        }

        private DetectCommandOptionsModel GetValidatedOptionsModel(CommandLineOptions options)
        {
            var startDateOption = options.GetOrNull(DetectCommandOptionsOptions.StartDate.Short,
                DetectCommandOptionsOptions.StartDate.Long);

            if (string.IsNullOrEmpty(startDateOption) ||
                !DateTime.TryParse(startDateOption, out var startDate))
            {
                throw new FlightTrackerUsageException(
                    $"<{DetectCommandOptionsOptions.StartDate.Long}> is missing or invalid!" +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }

            var endDateOption = options.GetOrNull(DetectCommandOptionsOptions.EndDate.Short,
                DetectCommandOptionsOptions.EndDate.Long);

            if (string.IsNullOrEmpty(endDateOption) ||
                !DateTime.TryParse(endDateOption, out var endDate))
            {
                throw new FlightTrackerUsageException(
                    $"<{DetectCommandOptionsOptions.EndDate.Long}> is missing or invalid!" +
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
                    $"<{DetectCommandOptionsOptions.AgencyId.Long}> is missing or invalid!" +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }

            return new DetectCommandOptionsModel(startDate, endDate, agencyId);
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine($"  flight-tracker {Name} " +
                          $"-{DetectCommandOptionsOptions.StartDate.Short} <{DetectCommandOptionsOptions.StartDate.Long}> " +
                          $"-{DetectCommandOptionsOptions.EndDate.Short} <{DetectCommandOptionsOptions.EndDate.Long}> " +
                          $"-{DetectCommandOptionsOptions.AgencyId.Short} <{DetectCommandOptionsOptions.AgencyId.Long}>");
            sb.AppendLine("");
            sb.AppendLine("Example:");
            sb.AppendLine("");
            sb.AppendLine($"  flight-tracker {Name} " +
                          $"-{DetectCommandOptionsOptions.StartDate.Short} 2018-01-01 " +
                          $"-{DetectCommandOptionsOptions.EndDate.Short} 2018-01-15 " +
                          $"-{DetectCommandOptionsOptions.AgencyId.Short} 1");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Detect flight schedule changes in airline competitive landscape.";
        }
    }
}