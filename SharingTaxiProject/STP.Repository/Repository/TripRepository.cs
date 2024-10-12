using Microsoft.EntityFrameworkCore;
using PMS.Repository.Base;
using STP.Repository.Models;

public class TripRepository : GenericRepository<Trip>
{
    public TripRepository(ShareTaxiContext context) : base(context) { }

    // Phương thức để lấy danh sách người tham gia chuyến đi
    public async Task<List<TripParticipant>> GetTripParticipantsAsync(int tripId)
    {
        return await _context.TripParticipants.Where(tp => tp.TripId == tripId).ToListAsync();
    }

    // Phương thức tính giá cho chuyến đi dựa trên số lượng người tham gia
    public async Task<decimal> CalculateTripPriceAsync(int tripTypeId, int numberOfParticipants)
    {
        var tripType = await _context.TripTypes.Include(tt => tt.TripeTypePricings)
            .FirstOrDefaultAsync(tt => tt.Id == tripTypeId);

        if (tripType == null) throw new Exception("Trip type not found.");

        // Tìm mức giá tương ứng dựa trên số người tham gia
        var pricingOptions = tripType.TripeTypePricings
            .Where(p => numberOfParticipants >= p.MinPerson && numberOfParticipants <= p.MaxPerson)
            .FirstOrDefault();

        if (pricingOptions == null) throw new Exception("No pricing available for this number of participants.");

        // Tính tổng giá
        decimal totalPrice = pricingOptions.PricePerPerson * numberOfParticipants;
        return totalPrice;
    }
}
