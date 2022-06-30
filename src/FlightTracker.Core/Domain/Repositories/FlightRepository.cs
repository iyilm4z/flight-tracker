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

            var destCityIds = subscriptions
                .Select(x => x.DestinationCityId)
                .ToList();

            var orgCityIds = subscriptions
                .Select(x => x.OriginCityId)
                .ToList();

            var query = _dbContext.Flights
                .Include(x => x.Route)
                .Where(x => x.DepartureTime >= startDate
                            && x.DepartureTime <= endDate
                            && destCityIds.Contains(x.Route.DestinationCityId)
                            && orgCityIds.Contains(x.Route.OriginCityId));

            return await query.ToListAsync();
        }

        public async Task<List<Flight>> GetFlightsAsync(DateTime startDate, DateTime endDate, int[] airlineIds)
        {
            // TODO don't do it here, or rename method name 
            var minStartDate = startDate.AddDays(-7).AddMinutes(-30);
            var maxEndDate = endDate.AddDays(7).AddMinutes(30);

            var query = _dbContext.Flights
                .Include(flight => flight.Route)
                .Where(flight => flight.DepartureTime >= minStartDate
                                 && flight.DepartureTime <= maxEndDate
                                 && airlineIds.Contains(flight.AirlineId));

            return await query.ToListAsync();
        }
    }
}