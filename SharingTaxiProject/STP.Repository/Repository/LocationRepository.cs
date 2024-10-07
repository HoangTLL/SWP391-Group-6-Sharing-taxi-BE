using PMS.Repository.Base; // Kế thừa từ GenericRepository
using STP.Repository.Models; // Tham chiếu đến model Location
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STP.Repository
{
    public class LocationRepository : GenericRepository<Location>
    {
        // Constructor nhận DbContext từ bên ngoài (có thể thông qua UnitOfWork)
        public LocationRepository(ShareTaxiContext context) : base(context)
        {
        }

        // Phương thức tìm kiếm Location theo tên
        public async Task<IEnumerable<Location>> GetLocationsByNameAsync(string name)
        {
            return await _context.Locations
                .Where(l => l.Name.Contains(name)) // Tìm kiếm các Location có tên chứa từ khóa
                .ToListAsync(); // Trả về danh sách kết quả
        }
    }
}
