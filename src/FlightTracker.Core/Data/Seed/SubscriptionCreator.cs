using System.Linq;
using FlightTracker.Csv.Read;
using FlightTracker.Domain.Entities;

namespace FlightTracker.Data.Seed
{
    public class SubscriptionCreator
    {
        private readonly FlightTrackerDbContext _context;
        private readonly FlightTrackerCsvReader _csvReader;

        public SubscriptionCreator(FlightTrackerDbContext context,
            FlightTrackerCsvReader csvReader)
        {
            _context = context;
            _csvReader = csvReader;
        }

        public void Create()
        {
            CreateSubscriptions();
        }

        private void CreateSubscriptions()
        {
            if (_context.Subscriptions.Any())
            {
                return;
            }
        
            var csvModels = _csvReader.ReadSubscriptions();

            var entities = csvModels.Select(csvModel => new Subscription
            {
                AgencyId = csvModel.AgencyId,
                DestinationCityId = csvModel.DestinationCityId,
                OriginCityId = csvModel.OriginCityId
            }).ToList();

            _context.Subscriptions.AddRange(entities);
        }
    }
}