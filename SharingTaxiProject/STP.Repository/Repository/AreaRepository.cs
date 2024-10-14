using Microsoft.EntityFrameworkCore;
using PMS.Repository.Base; // Sử dụng lớp GenericRepository từ thư viện PMS.Repository.Base
using STP.Repository.Models; // Tham chiếu đến model Area từ namespace STP.Repository.Models
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STP.Repository.Repository
{
    // Lớp AreaRepository kế thừa từ GenericRepository với kiểu thực thể là Area
    public class AreaRepository : GenericRepository<Area>
    {
        // Constructor nhận ShareTaxiContext (DbContext) và gán cho _context
        public AreaRepository(ShareTaxiContext context) => _context = context;

        // Phương thức bất đồng bộ để lấy danh sách tất cả các Area (khu vực) bao gồm cả các Locations (địa điểm) và Trips (chuyến đi)
        public async Task<List<Area>> GetAllAsync()
        {
            return await _context.Areas  // Truy vấn tất cả các Area trong bảng Areas
                .Include(a => a.Locations) // Bao gồm thêm các Locations liên quan
                .Include(a => a.Trips) // Bao gồm thêm các Trips liên quan
                .ToListAsync(); // Trả về danh sách dưới dạng bất đồng bộ
        }

        // Phương thức bất đồng bộ để lấy một Area cụ thể bằng id, bao gồm cả các Locations và Trips liên quan
        public async Task<Area> GetByIdAsync(int id)
        {
            // Truy vấn Area với Id cụ thể và bao gồm thêm thông tin về Locations và Trips
            var result = await _context.Areas
                .Include(a => a.Locations) // Bao gồm thêm các Locations liên quan
                .Include(a => a.Trips) // Bao gồm thêm các Trips liên quan
                .FirstOrDefaultAsync(a => a.Id == id); // Tìm và trả về Area có Id khớp với tham số

            return result; // Trả về kết quả truy vấn
        }
    }
}
