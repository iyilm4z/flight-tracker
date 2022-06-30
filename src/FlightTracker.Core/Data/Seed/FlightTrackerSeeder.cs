using FlightTracker.Csv.Import;

namespace FlightTracker.Data.Seed
{
    public class FlightTrackerSeeder
    {
        private readonly FlightTrackerDbContext _context;

        public FlightTrackerSeeder(FlightTrackerDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            var cvsReader = new FlightTrackerCsvReader();

            new SubscriptionCreator(_context, cvsReader).Create();
            new RouteCreator(_context, cvsReader).Create();
            new FlightCreator(_context, cvsReader).Create();

            _context.SaveChanges();
        }
    }
}