using System;
using FlightTracker.Args;
using FlightTracker.Commands.Help;

namespace FlightTracker.Commands
{
    public class CommandSelector : ICommandSelector
    {
        protected FlightTrackerSettings Options { get; }

        public CommandSelector(FlightTrackerSettings options)
        {
            Options = options;
        }

        public Type Select(CommandLineArgs commandLineArgs)
        {
            if (string.IsNullOrWhiteSpace(commandLineArgs.Command))
            {
                return typeof(HelpCommand);
            }

            return Options.Commands.TryGetValue(commandLineArgs.Command, out var obj)
                ? obj
                : typeof(HelpCommand);
        }
    }
}