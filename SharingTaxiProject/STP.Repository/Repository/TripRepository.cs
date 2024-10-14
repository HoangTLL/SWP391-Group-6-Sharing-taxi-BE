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
    // Phương thức tính giá chuyến đi dựa trên PickUpLocationId, DropOffLocationId và số người tham gia
    public async Task<decimal> CalculateTripPrice(int pickUpLocationId, int dropOffLocationId, int numberOfParticipants)
    {
        // Bước 1: Lấy thông tin Location của điểm đón và trả
        var pickUpLocation = await _context.Locations.FirstOrDefaultAsync(l => l.Id == pickUpLocationId);
        var dropOffLocation = await _context.Locations.FirstOrDefaultAsync(l => l.Id == dropOffLocationId);

        if (pickUpLocation == null || dropOffLocation == null)
        {
            throw new Exception("Pick-up or drop-off location not found.");
        }

        // Bước 2: Lấy FromAreaId từ PickUpLocation và ToAreaId từ DropOffLocation
        int fromAreaId = pickUpLocation.AreaId ?? throw new Exception("Pick-up location does not have an associated area.");
        int toAreaId = dropOffLocation.AreaId ?? throw new Exception("Drop-off location does not have an associated area.");

        // Bước 3: Tìm thông tin về TripType dựa trên FromAreaId và ToAreaId
        var tripType = await _context.TripTypes
            .FirstOrDefaultAsync(tt => tt.FromAreaId == fromAreaId && tt.ToAreaId == toAreaId);

        if (tripType == null)
        {
            throw new Exception("Trip type not found for the specified areas.");
        }

        // Bước 4: Lấy thông tin giá từ bảng TripTypePricing dựa trên số người tham gia
        var pricing = await _context.TripTypePricings
            .FirstOrDefaultAsync(p => p.TripType == tripType.Id && numberOfParticipants >= p.MinPerson && numberOfParticipants <= p.MaxPerson);

        if (pricing == null)
        {
            throw new Exception("Pricing not available for the specified number of participants.");
        }

        // Bước 5: Tính tổng chi phí dựa trên số lượng người tham gia
        decimal totalCost = pricing.PricePerPerson * numberOfParticipants;

        return totalCost;
    }
}
