using System.Linq;
using FlightTracker.Csv.Read;
using FlightTracker.Domain.Entities;

namespace FlightTracker.Data.Seed
{
    public class RouteCreator
    {
        private readonly FlightTrackerDbContext _context;
        private readonly FlightTrackerCsvReader _csvReader;

        public RouteCreator(FlightTrackerDbContext context,
            FlightTrackerCsvReader csvReader)
        {
            _context = context;
            _csvReader = csvReader;
        }

        public void Create()
        {
            CreateRoutes();
        }

        private void CreateRoutes()
        {
            if (_context.Routes.Any())
            {
                return;
            }

            var csvModels = _csvReader.ReadRoutes();

            var entities = csvModels.Select(csvModel => new Route
            {
                Id = csvModel.RouteId,
                DestinationCityId = csvModel.DestinationCityId,
                OriginCityId = csvModel.OriginCityId,
                DepartureDate = csvModel.DepartureDate
            }).ToList();

            _context.Routes.AddRange(entities);
        }
    }
}