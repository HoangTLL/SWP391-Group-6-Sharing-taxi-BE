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
        public async Task<List<object>> GetTripsByUserIdAsync(int userId)
        {
            return await _context.Trips
                .Where(t => t.Bookings.Any(b => b.UserId == userId))
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
        public async Task<object> GetTripStatisticsAsync()
        {
            var threeMonthsAgo = DateOnly.FromDateTime(DateTime.Now.AddMonths(-3));

            // Lấy thông tin chuyến đi được tạo nhiều nhất (địa điểm đón và trả khách)
            var mostCreatedTrip = await _context.Trips
                .Where(t => t.BookingDate >= threeMonthsAgo)
                .GroupBy(t => new { t.PickUpLocationId, t.DropOffLocationId })
                .Select(g => new
                {
                    From = g.First().PickUpLocation.Name,
                    To = g.First().DropOffLocation.Name,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .FirstOrDefaultAsync();

            // Số chuyến đi tạo mỗi tháng trong 3 tháng gần nhất
            var tripsPerMonth = await _context.Trips
                .Where(t => t.BookingDate >= threeMonthsAgo)
                .GroupBy(t => new { t.BookingDate.Value.Year, t.BookingDate.Value.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .ToListAsync();

            // Số chuyến đi bị hủy (status = 0) mỗi tháng trong 3 tháng gần nhất
            var inactiveTripsPerMonth = await _context.Trips
                .Where(t => t.Status == 0 && t.BookingDate >= threeMonthsAgo)
                .GroupBy(t => new { t.BookingDate.Value.Year, t.BookingDate.Value.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .ToListAsync();

            // Thống kê số lượng người tham gia theo tháng
            var participantsPerMonth = await _context.Bookings
            .Include(b => b.Trip)
            .Where(b => b.Trip.BookingDate >= threeMonthsAgo
                     && b.Trip.Status == 1)  // Chỉ lấy các trip có status = 1
            .GroupBy(b => new
            {
                Year = b.Trip.BookingDate.Value.Year,
                Month = b.Trip.BookingDate.Value.Month
            })
            .Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalParticipants = g.Count()
            })
            .ToListAsync();

            return new
            {
                MostCreatedTrip = mostCreatedTrip,
                TripsPerMonth = tripsPerMonth,
                InactiveTripsPerMonth = inactiveTripsPerMonth,
                ParticipantsStatistics = participantsPerMonth
            };
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