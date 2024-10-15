using Microsoft.EntityFrameworkCore;
using PMS.Repository.Base;
using STP.Repository.Models;

public class TripRepository : GenericRepository<Trip>
{
    public TripRepository(ShareTaxiContext context) : base(context) { }

    // Phương thức để lấy danh sách người tham gia chuyến đi (thông qua bảng Booking)
    public async Task<List<Booking>> GetTripParticipantsAsync(int tripId)
    {
        return await _context.Bookings.Where(b => b.TripId == tripId).ToListAsync();
    }

    // Phương thức tính giá chuyến đi dựa trên PickUpLocationId, DropOffLocationId và số người tham gia
    public async Task<decimal> CalculateTripPrice(int tripId, int numberOfParticipants)
    {
        // Lấy thông tin chuyến đi
        var trip = await _context.Trips.FirstOrDefaultAsync(t => t.Id == tripId);
        if (trip == null)
        {
            throw new Exception("Trip not found.");
        }

        // Lấy thông tin TripTypePricing dựa trên TripTypeId của chuyến đi
        var pricing = await _context.TripTypePricings
            .FirstOrDefaultAsync(p => p.TripType == trip.TripTypeId && numberOfParticipants >= p.MinPerson && numberOfParticipants <= p.MaxPerson);

        if (pricing == null)
        {
            throw new Exception("Pricing not available for the specified number of participants.");
        }

        // Tính tổng chi phí dựa trên số người tham gia nhưng theo giá MinPerson
        decimal totalCost = pricing.PricePerPerson * numberOfParticipants;

        return totalCost;
    
}
    public async Task<Trip> GetTripWithPricingAsync(int tripId)
    {
        // Lấy thông tin Trip và liên kết với bảng TripType và TripTypePricing
        return await _context.Trips
            .Include(t => t.TripType) // Liên kết với TripType
            .Include(t => t.TripType.TripTypePricings) // Liên kết với TripTypePricing
            .FirstOrDefaultAsync(t => t.Id == tripId);
    }

}
