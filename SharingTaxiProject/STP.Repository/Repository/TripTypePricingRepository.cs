using Microsoft.EntityFrameworkCore;
using STP.Repository.Models;
using PMS.Repository.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace STP.Repository
{
    public class TripTypePricingRepository : GenericRepository<TripTypePricing>
    {
        private readonly ILogger _logger;

        public TripTypePricingRepository(ShareTaxiContext context, ILogger logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<TripTypePricing> GetPricingForTripAsync(int tripTypeId, int numberOfParticipants)
        {
            _logger.LogInformation($"Searching pricing for TripTypeId: {tripTypeId}, Participants: {numberOfParticipants}");

            var allPricings = await _context.TripTypePricings
                .Where(ttp => ttp.TripType == tripTypeId)
                .ToListAsync();

            _logger.LogInformation($"Found {allPricings.Count} pricing records for TripTypeId: {tripTypeId}");

            foreach (var pricing in allPricings)
            {
                _logger.LogInformation($"Pricing: Id={pricing.Id}, MinPerson={pricing.MinPerson}, MaxPerson={pricing.MaxPerson}, Price={pricing.PricePerPerson}");
            }

            var exactMatch = allPricings
                .FirstOrDefault(ttp => ttp.MinPerson <= numberOfParticipants && ttp.MaxPerson >= numberOfParticipants);

            if (exactMatch != null)
            {
                _logger.LogInformation($"Found exact match: Id={exactMatch.Id}, Price={exactMatch.PricePerPerson}");
                return exactMatch;
            }

            // If no exact match, find the closest match
            var closestMatch = allPricings
                .OrderBy(ttp => Math.Abs(ttp.MinPerson - numberOfParticipants))
                .ThenBy(ttp => ttp.PricePerPerson)
                .FirstOrDefault();

            if (closestMatch != null)
            {
                _logger.LogInformation($"Found closest match: Id={closestMatch.Id}, Price={closestMatch.PricePerPerson}");
                return closestMatch;
            }

            _logger.LogWarning($"No pricing found for TripTypeId: {tripTypeId}, Participants: {numberOfParticipants}");
            return null;
        }

        public async Task<TripTypePricing> GetInitialPricingForTripAsync(int fromAreaId, int toAreaId)
        {
            return await _context.TripTypes
                .Where(tt => tt.FromAreaId == fromAreaId && tt.ToAreaId == toAreaId)
                .SelectMany(tt => tt.TripTypePricings)
                .OrderBy(ttp => ttp.MinPerson)
                .FirstOrDefaultAsync();
        }

        public async Task<(decimal TotalCost, decimal PricePerPerson, int MinPerson, int MaxPerson)> CalculateTripPrice(int tripId)
        {
            var trip = await _context.Trips
                .Include(t => t.TripType)
                .Include(t => t.Bookings)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null)
            {
                throw new Exception("Không tìm thấy chuyến đi.");
            }

            int actualParticipants = trip.Bookings.Count;

            // Lấy thông tin giá dựa trên MinPerson của Trip
            var pricing = await _context.TripTypePricings
                .Where(ttp => ttp.TripType == trip.TripTypeId && ttp.MinPerson == trip.MinPerson)
                .FirstOrDefaultAsync();

            if (pricing == null)
            {
                throw new Exception("Không tìm thấy giá phù hợp cho chuyến đi.");
            }

            decimal pricePerPerson = pricing.PricePerPerson;
            // Tính tổng chi phí dựa trên số người tham gia thực tế, nhưng không ít hơn MinPerson
            int chargedParticipants = Math.Max(actualParticipants, trip.MinPerson ?? 1);
            decimal totalCost = pricePerPerson * chargedParticipants;

            return (totalCost, pricePerPerson, trip.MinPerson ?? 1, trip.MaxPerson ?? chargedParticipants);
        }

        public async Task<List<TripTypePricing>> GetAllTripTypePricingsAsync()
        {
            return await _context.TripTypePricings
                .Include(ttp => ttp.TripTypeNavigation)
                .ToListAsync();
        }

        public async Task<TripTypePricing> GetTripTypePricingByIdAsync(int id)
        {
            return await _context.TripTypePricings
                .Include(ttp => ttp.TripTypeNavigation)
                .FirstOrDefaultAsync(ttp => ttp.Id == id);
        }

        public async Task<bool> TripTypeExistsAsync(int tripTypeId)
        {
            return await _context.TripTypes.AnyAsync(tt => tt.Id == tripTypeId);
        }

        public async Task<List<TripTypePricing>> GetTripTypePricingsByTripTypeAsync(int tripTypeId)
        {
            return await _context.TripTypePricings
                .Where(ttp => ttp.TripType == tripTypeId)
                .ToListAsync();
        }

        public async Task<bool> ExistsWithSameParametersAsync(int tripType, int minPerson, int maxPerson, int? excludeId = null)
        {
            return await _context.TripTypePricings
                .AnyAsync(ttp =>
                    ttp.TripType == tripType &&
                    ttp.MinPerson == minPerson &&
                    ttp.MaxPerson == maxPerson &&
                    (excludeId == null || ttp.Id != excludeId));
        }
    }
}
