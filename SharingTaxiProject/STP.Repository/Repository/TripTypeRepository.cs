using PMS.Repository.Base;
using STP.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STP.Repository
{
    public class TripTypeRepository : GenericRepository<TripType>
    {
        public TripTypeRepository(ShareTaxiContext context) : base(context)
        {
        }

        // Get trip type by pickup and drop-off areas
        public async Task<TripType> GetTripTypeByAreasAsync(int fromAreaId, int toAreaId)
        {
            return await _context.TripTypes
                .FirstOrDefaultAsync(t => t.FromAreaId == fromAreaId && t.ToAreaId == toAreaId);
        }

        public async Task<List<TripType>> GetAllTripTypesWithPricingsAsync()
        {
            return await _context.TripTypes
                .Include(tt => tt.TripTypePricings)
                .ToListAsync();
        }

        public async Task<TripType> GetTripTypeWithPricingsByIdAsync(int id)
        {
            return await _context.TripTypes
                .Include(tt => tt.TripTypePricings)
                .FirstOrDefaultAsync(tt => tt.Id == id);
        }

        public async Task AddTripTypePricingAsync(TripTypePricing pricing)
        {
            await _context.TripTypePricings.AddAsync(pricing);
        }

        public void UpdateTripTypePricing(TripTypePricing pricing)
        {
            _context.TripTypePricings.Update(pricing);
        }

        public async Task<TripTypePricing> GetTripTypePricingByIdAsync(int id)
        {
            return await _context.TripTypePricings.FindAsync(id);
        }

        public async Task<bool> AreaExistsAsync(int areaId)
        {
            return await _context.Areas.AnyAsync(a => a.Id == areaId);
        }

        public async Task<bool> TripTypeExistsAsync(int fromAreaId, int toAreaId, int? excludeId = null)
        {
            return await _context.TripTypes
                .AnyAsync(tt => tt.FromAreaId == fromAreaId &&
                                tt.ToAreaId == toAreaId &&
                                (excludeId == null || tt.Id != excludeId));
        }
    }
}
