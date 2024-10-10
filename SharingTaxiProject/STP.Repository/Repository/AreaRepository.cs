using Microsoft.EntityFrameworkCore;
using PMS.Repository.Base;
<<<<<<< Updated upstream
using STP.Repository.Models;
=======

using STP.Repository.Models; // Reference to the Area model
>>>>>>> Stashed changes
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STP.Repository.Repository
{
<<<<<<< Updated upstream
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
=======
    // AreaRepository inherits from GenericRepository with the Area entity type
    public class AreaRepository : GenericRepository<Area>
    {
        // Constructor that accepts a ShareTaxiContext (DbContext) and passes it to the base class
        public AreaRepository(ShareTaxiContext context) : base(context) { }

        // Asynchronous method to retrieve all Areas, including related Locations
        public async Task<List<Area>> GetAllAsync()
        {
            return await _context.Areas
                .Include(a => a.Locations) // Include related Locations
                .ToListAsync(); // Return the list asynchronously
        }

        // Asynchronous method to retrieve a specific Area by its ID, including related Locations
        public async Task<Area> GetByIdAsync(int id)
        {
            return await _context.Areas
                .Include(a => a.Locations) // Include related Locations
                .FirstOrDefaultAsync(a => a.Id == id); // Return the Area that matches the ID
>>>>>>> Stashed changes
        }
    }
}
