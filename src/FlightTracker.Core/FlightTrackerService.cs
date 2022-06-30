using System;
using System.Threading.Tasks;
using FlightTracker.Args;
using FlightTracker.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FlightTracker
{
    public class FlightTrackerService
    {
        private readonly ILogger<FlightTrackerService> _logger;
        private readonly ICommandLineArgumentParser _commandLineArgumentParser;
        private readonly ICommandSelector _commandSelector;

        public FlightTrackerService(ILogger<FlightTrackerService> logger,
            ICommandLineArgumentParser commandLineArgumentParser,
            ICommandSelector commandSelector)
        {
            _logger = logger;
            _commandLineArgumentParser = commandLineArgumentParser;
            _commandSelector = commandSelector;
        }

        public async Task RunAsync(string[] args)
        {
            var commandLineArgs = _commandLineArgumentParser.Parse(args);

            var commandType = _commandSelector.Select(commandLineArgs);

            using var scope = ServiceProviderHelper.Instance.Services.CreateScope();
            try
            {
                var command = (IConsoleCommand)scope.ServiceProvider.GetRequiredService(commandType);
                await command.ExecuteAsync(commandLineArgs);
            }
            catch (FlightTrackerUsageException usageException)
            {
                _logger.LogWarning(usageException.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred!", ex);
            }
        }
    }
}