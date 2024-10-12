using Microsoft.EntityFrameworkCore;
using PMS.Repository.Base;
using STP.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STP.Repository.Repository
{
    public class CarTripRepository : GenericRepository<CarTrip>
    {
        public CarTripRepository(ShareTaxiContext context) : base(context) { }

        public async Task<List<CarTrip>> GetAllCarTripsWithTripAsync()
        {
            return await _context.CarTrips
                .Include(ct => ct.Trip)
                .ToListAsync();
        }

        public async Task<CarTrip> GetCarTripWithTripByIdAsync(int id)
        {
            return await _context.CarTrips
                .Include(ct => ct.Trip)
                .FirstOrDefaultAsync(ct => ct.Id == id);
        }

        public async Task<CarTrip> GetByTripIdAsync(int tripId)
        {
            return await _context.CarTrips
                .Include(ct => ct.Trip)
                .FirstOrDefaultAsync(ct => ct.TripId == tripId);
        }

        public async Task AddAsync(CarTrip carTrip)
        {
            await _context.CarTrips.AddAsync(carTrip);
        }
    }
}