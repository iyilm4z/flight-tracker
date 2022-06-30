using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightTracker.Args;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FlightTracker.Commands.Help
{
    public class HelpCommand : IConsoleCommand
    {
        public const string Name = "help";

        private readonly ILogger<HelpCommand> _logger;
        private readonly FlightTrackerSettings _flightTrackerSettings;

        public HelpCommand(ILogger<HelpCommand> logger,
            FlightTrackerSettings flightTrackerSettings)
        {
            _logger = logger;
            _flightTrackerSettings = flightTrackerSettings;
        }

        public Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (string.IsNullOrWhiteSpace(commandLineArgs.Target))
            {
                _logger.LogInformation(GetUsageInfo());

                return Task.CompletedTask;
            }

            if (!_flightTrackerSettings.Commands.ContainsKey(commandLineArgs.Target))
            {
                _logger.LogWarning($"There is no command named {commandLineArgs.Target}.");
                _logger.LogInformation(GetUsageInfo());

                return Task.CompletedTask;
            }

            var commandType = _flightTrackerSettings.Commands[commandLineArgs.Target];

            using (var scope = ServiceProviderHelper.Instance.Services.CreateScope())
            {
                var command = (IConsoleCommand)scope.ServiceProvider.GetRequiredService(commandType);
                _logger.LogInformation(command.GetUsageInfo());
            }

            return Task.CompletedTask;
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("    flight-tracker <command> <target?> [options?]");
            sb.AppendLine("");
            sb.AppendLine("Command List:");
            sb.AppendLine("");

            foreach (var command in _flightTrackerSettings.Commands.ToArray())
            {
                string shortDescription;

                using (var scope = ServiceProviderHelper.Instance.Services.CreateScope())
                {
                    shortDescription = ((IConsoleCommand)scope.ServiceProvider.GetRequiredService(command.Value))
                        .GetShortDescription();
                }

                sb.Append("    > ");
                sb.Append(command.Key);
                sb.Append(string.IsNullOrWhiteSpace(shortDescription) ? "" : ":");
                sb.Append(" ");
                sb.AppendLine(shortDescription);
            }

            sb.AppendLine("");
            sb.AppendLine("To get a detailed help for a command:");
            sb.AppendLine("");
            sb.AppendLine("    flight-tracker help <command>");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Show command line help. Write ` flight-tracker help <command> `";
        }
    }
}