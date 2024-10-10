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

        // Additional methods can be added here as needed
    }
}
