using Microsoft.EntityFrameworkCore;
using PMS.Repository.Base;
using STP.Repository.Models;
using System.Threading.Tasks;

namespace STP.Repository
{
    public class BookingRepository : GenericRepository<Booking>
    {
        public BookingRepository(ShareTaxiContext context) : base(context)
        {
        }

        public async Task<Booking> GetBookingByUserAndTripAsync(int userId, int tripId)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.TripId == tripId);
        }

        public async Task<bool> CreateBookingAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteBookingAsync(Booking booking)
        {
            _context.Bookings.Remove(booking);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}