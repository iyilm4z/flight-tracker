using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightTracker.Data;
using FlightTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightTracker.Domain.Repositories
{
    public class FlightRepository
    {
        private readonly FlightTrackerDbContext _dbContext;

        public FlightRepository(FlightTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Flight>> GetFlightsAsync(DateTime startDate, DateTime endDate, int agencyId)
        {
            var subscriptions = _dbContext.Subscriptions
                .Where(x => x.AgencyId == agencyId)
                .ToList();

            var originCityIds = subscriptions
                .Select(x => x.OriginCityId)
                .ToList();

            var destinationCityIds = subscriptions
                .Select(x => x.DestinationCityId)
                .ToList();

            var query = _dbContext.Flights
                .Include(x => x.Route)
                .Where(x => x.DepartureTime >= startDate
                            && x.DepartureTime <= endDate
                            && originCityIds.Contains(x.Route.OriginCityId)
                            && destinationCityIds.Contains(x.Route.DestinationCityId));

            return await query.ToListAsync();
        }
    }
}