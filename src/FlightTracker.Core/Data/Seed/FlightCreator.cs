using System.Linq;
using FlightTracker.Csv.Import;
using FlightTracker.Domain.Entities;

namespace FlightTracker.Data.Seed
{
    public class FlightCreator
    {
        private readonly FlightTrackerDbContext _context;
        private readonly FlightTrackerCsvReader _csvReader;

        public FlightCreator(FlightTrackerDbContext context,
            FlightTrackerCsvReader csvReader)
        {
            _context = context;
            _csvReader = csvReader;
        }

        public void Create()
        {
            CreateFlights();
        }

        private void CreateFlights()
        {
            if (_context.Flights.Any())
            {
                return;
            }

            var csvModels = _csvReader.ReadFlights();

            var entities = csvModels.Select(csvModel => new Flight
            {
                Id = csvModel.FlightId,
                AirlineId = csvModel.AirlineId,
                ArrivalTime = csvModel.ArrivalTime,
                DepartureTime = csvModel.DepartureTime,
                RouteId = csvModel.RouteId
            }).ToList();
            
           _context.Flights.AddRange(entities);
        }
    }
}