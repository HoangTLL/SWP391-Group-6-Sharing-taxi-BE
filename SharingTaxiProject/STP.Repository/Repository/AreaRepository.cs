using Microsoft.EntityFrameworkCore;
using PMS.Repository.Base;
using STP.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STP.Repository.Repository
{
    public class AreaRepository : GenericRepository<Area>
    {
        public AreaRepository(ShareTaxiContext context) => _context = context;

        // Get all areas with related locations and trips
        public async Task<List<Area>> GetAllAsync()
        {
            return await _context.Areas
                .Include(a => a.Locations)
                .Include(a => a.Trips)
                .ToListAsync();
        }

        // Get a specific area by id with related locations and trips
        public async Task<Area> GetByIdAsync(int id)
        {
            var result = await _context.Areas
                .Include(a => a.Locations)
                .Include(a => a.Trips)
                .FirstOrDefaultAsync(a => a.Id == id);

            return result;
        }
    }
}
