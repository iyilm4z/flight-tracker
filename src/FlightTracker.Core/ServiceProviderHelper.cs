using System;

namespace FlightTracker
{
    public class ServiceProviderHelper
    {
        private readonly object _padlock = new();

        private IServiceProvider _serviceProvider;

        private ServiceProviderHelper()
        {
        }

        public static ServiceProviderHelper Instance { get; protected set; } = new();

        public IServiceProvider Services
        {
            get => _serviceProvider;
            set
            {
                if (_serviceProvider != null)
                {
                    return;
                }

                lock (_padlock)
                {
                    _serviceProvider ??= value;
                }
            }
        }
    }
}