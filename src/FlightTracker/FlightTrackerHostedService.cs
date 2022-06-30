using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace FlightTracker
{
    internal class FlightTrackerHostedService : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostLifetime;
        private readonly FlightTrackerService _flightTrackerService;

        public FlightTrackerHostedService(IHostApplicationLifetime hostLifetime,
            FlightTrackerService flightTrackerService)
        {
            _hostLifetime = hostLifetime;
            _flightTrackerService = flightTrackerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var args = Environment.GetCommandLineArgs();

            var argumentList = args.ToList();

            // The first element is the executable file name.
            // We don't need it. So, remove it.
            argumentList.RemoveAt(0);

            await _flightTrackerService.RunAsync(argumentList.ToArray());

            _hostLifetime.StopApplication();
        }
    }
}