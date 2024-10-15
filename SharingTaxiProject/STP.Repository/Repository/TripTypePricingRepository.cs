using Microsoft.EntityFrameworkCore;
using STP.Repository.Models;
using PMS.Repository.Base;
using System.Linq;
using System.Threading.Tasks;

namespace STP.Repository
{
    public class TripTypePricingRepository : GenericRepository<TripTypePricing>
    {
        public TripTypePricingRepository(ShareTaxiContext context) : base(context)
        {
        }

        public async Task<TripTypePricing> GetPricingForTripAsync(int tripTypeId, int numberOfParticipants)
        {
            return await _context.TripTypePricings
                .Where(ttp => ttp.TripType == tripTypeId &&
                              ttp.MinPerson <= numberOfParticipants &&
                              ttp.MaxPerson >= numberOfParticipants)
                .FirstOrDefaultAsync();
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
    }
}