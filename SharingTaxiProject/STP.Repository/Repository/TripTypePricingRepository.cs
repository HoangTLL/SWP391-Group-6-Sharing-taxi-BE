using Microsoft.EntityFrameworkCore;
using STP.Repository.Models;
using PMS.Repository.Base; // Tham chiếu đến lớp GenericRepository
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STP.Repository
{
    // TripeTypePricingRepository kế thừa từ GenericRepository<TripeTypePricing>
    public class TripTypePricingRepository : GenericRepository<TripTypePricing>
    {
        // Constructor nhận vào ShareTaxiContext từ UnitOfWork hoặc khởi tạo DbContext
        public TripTypePricingRepository(ShareTaxiContext context) : base(context)
        {
        }

        // Phương thức để lấy danh sách các mức giá theo loại chuyến đi
        public async Task<List<TripTypePricing>> GetPricingByTripTypeAsync(int tripTypeId)
        {
            return await _context.TripTypePricings
                                 .Where(p => p.TripType == tripTypeId)
                                 .ToListAsync();
        }

        // Phương thức lấy giá cụ thể theo số lượng người tham gia
        public async Task<TripTypePricing> GetPricingByParticipantsAsync(int tripTypeId, int numberOfParticipants)
        {
            return await _context.TripTypePricings
                                 .Where(p => p.TripType == tripTypeId && p.MinPerson <= numberOfParticipants && p.MaxPerson >= numberOfParticipants)
                                 .FirstOrDefaultAsync();
        }
    }
}
