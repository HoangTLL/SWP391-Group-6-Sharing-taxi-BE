using Microsoft.EntityFrameworkCore;
using PMS.Repository.Base;
using STP.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STP.Repository
{
    public class TripRepository : GenericRepository<Trip>
    {
        public TripRepository(ShareTaxiContext context) : base(context) { }

        // Method to add a new trip asynchronously
        public async Task AddAsync(Trip trip)
        {
            await _context.Trips.AddAsync(trip);
            await _context.SaveChangesAsync();
        }

        // Method to retrieve available trips with location names
        public async Task<List<object>> GetAvailableTripsAsync()
        {
            return await _context.Trips
                .Where(t => t.Status == 1 && t.MaxPerson > t.Bookings.Count)
                .Include(t => t.PickUpLocation)
                .Include(t => t.DropOffLocation)
                .Select(t => new
                {
                    t.Id,
                    PickUpLocationName = t.PickUpLocation.Name,
                    DropOffLocationName = t.DropOffLocation.Name,
                    t.ToAreaId,
                    t.MaxPerson,
                    t.MinPerson,
                    t.UnitPrice,
                    t.BookingDate,
                    t.HourInDay,
                    t.Status
                })
                .ToListAsync<object>();
        }

        // Additional method to get trip by id with location names
        public async Task<Trip> GetTripByIdAsync(int id)
        {
            return await _context.Trips
                .Include(t => t.PickUpLocation)
                .Include(t => t.DropOffLocation)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // Method to get simplified Trip object with location names by id
        public async Task<object> GetTripByIdWithLocationNamesAsync(int id)
        {
            return await _context.Trips
                .Where(t => t.Id == id)
                .Include(t => t.PickUpLocation)
                .Include(t => t.DropOffLocation)
                .Select(t => new
                {
                    t.Id,
                    PickUpLocationName = t.PickUpLocation.Name,
                    DropOffLocationName = t.DropOffLocation.Name,
                    t.ToAreaId,
                    t.MaxPerson,
                    t.MinPerson,
                    t.UnitPrice,
                    t.BookingDate,
                    t.HourInDay,
                    t.TripTypeId,
                    t.Status
                })
                .FirstOrDefaultAsync();
        }

        // Search trips by location names
        public async Task<List<object>> SearchTripsAsync(
            int? pickUpLocationId,
            int? dropOffLocationId,
            DateOnly? bookingDate,
            TimeOnly? hourInDay,
            int? availableSlots)
        {
            var query = _context.Trips.Where(t => t.Status == 1).AsQueryable();

            if (pickUpLocationId.HasValue)
            {
                query = query.Where(t => t.PickUpLocationId == pickUpLocationId);
            }
            if (dropOffLocationId.HasValue)
            {
                query = query.Where(t => t.DropOffLocationId == dropOffLocationId);
            }
            if (bookingDate.HasValue)
            {
                query = query.Where(t => t.BookingDate == bookingDate);
            }
            if (hourInDay.HasValue)
            {
                query = query.Where(t => t.HourInDay == hourInDay);
            }
            if (availableSlots.HasValue)
            {
                query = query.Where(t => t.MaxPerson > t.Bookings.Count &&
                                         (t.MaxPerson - t.Bookings.Count) >= availableSlots);
            }

            return await query
                .Include(t => t.PickUpLocation)
                .Include(t => t.DropOffLocation)
                .Select(t => new
                {
                    t.Id,
                    PickUpLocationName = t.PickUpLocation.Name,
                    DropOffLocationName = t.DropOffLocation.Name,
                    t.ToAreaId,
                    t.MaxPerson,
                    t.MinPerson,
                    t.UnitPrice,
                    t.BookingDate,
                    t.HourInDay,
                    t.Status,
                    AvailableSlots = t.MaxPerson - t.Bookings.Count
                })
                .ToListAsync<object>();
        }

        // Method to retrieve all trips with location names
        public async Task<List<object>> GetAllTripsAsync()
        {
            return await _context.Trips
                .Include(t => t.PickUpLocation)
                .Include(t => t.DropOffLocation)
                .Select(t => new
                {
                    t.Id,
                    PickUpLocationName = t.PickUpLocation.Name,
                    DropOffLocationName = t.DropOffLocation.Name,
                    t.ToAreaId,
                    t.MaxPerson,
                    t.MinPerson,
                    t.UnitPrice,
                    t.BookingDate,
                    t.HourInDay,
                    t.TripTypeId,
                    t.Status
                })
                .ToListAsync<object>();
        }
    }
}