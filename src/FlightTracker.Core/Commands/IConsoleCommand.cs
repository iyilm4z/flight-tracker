using System.Threading.Tasks;
using FlightTracker.Args;

namespace FlightTracker.Commands
{
    public interface IConsoleCommand
    {
        Task ExecuteAsync(CommandLineArgs commandLineArgs);

        string GetUsageInfo();

        string GetShortDescription();
    }
}