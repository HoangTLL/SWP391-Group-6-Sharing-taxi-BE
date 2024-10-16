using Microsoft.EntityFrameworkCore;
using PMS.Repository.Base;
using STP.Repository.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STP.Repository
{
    public class BookingRepository : GenericRepository<Booking>
    {
        public BookingRepository(ShareTaxiContext context) : base(context)
        {
        }

        // Retrieve a booking by userId and tripId
        public async Task<Booking> GetBookingByUserAndTripAsync(int userId, int tripId)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.TripId == tripId);
        }

        // Count bookings by tripId
        public async Task<int> CountBookingsByTripIdAsync(int tripId)
        {
            return await _context.Bookings
                .CountAsync(b => b.TripId == tripId);
        }

        // Create a new booking
        public async Task<bool> CreateBookingAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            return await _context.SaveChangesAsync() > 0;
        }

        // Retrieve all bookings for a specific tripId, including user details
        public async Task<IEnumerable<Booking>> GetBookingsByTripIdAsync(int tripId)
        {
            return await _context.Bookings
                .Include(b => b.User) // Include user details
                .Where(b => b.TripId == tripId)
                .ToListAsync();
        }

        // Delete a booking
        public async Task<bool> DeleteBookingAsync(Booking booking)
        {
            _context.Bookings.Remove(booking);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
