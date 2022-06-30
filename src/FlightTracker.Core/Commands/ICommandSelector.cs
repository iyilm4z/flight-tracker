using System;
using FlightTracker.Args;

namespace FlightTracker.Commands
{
    public interface ICommandSelector
    {
        Type Select(CommandLineArgs commandLineArgs);
    }
}