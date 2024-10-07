using Microsoft.EntityFrameworkCore;
using PMS.Repository.Base; // Sử dụng lớp GenericRepository từ thư viện PMS.Repository.Base
using STP.Repository.Models; // Tham chiếu đến model User từ namespace STP.Repository.Models
using System.Threading.Tasks;

namespace STP.Repository.Repository
{
    // Lớp UserRepository kế thừa từ GenericRepository với kiểu thực thể là User
    public class UserRepository : GenericRepository<User>
    {
        // Constructor nhận ShareTaxiContext (DbContext) và gán cho _context
        public UserRepository(ShareTaxiContext context) => _context = context;

        // Phương thức bất đồng bộ để lấy một User dựa trên email
        public async Task<User> GetByEmailAsync(string email)
        {
            // Truy vấn tìm user với email khớp, nếu không tìm thấy trả về null
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        // Phương thức bất đồng bộ để lấy một User dựa trên email và password
        public async Task<User> GetByEmailAndPasswordAsync(string email, string password)
        {
            // Truy vấn tìm user với email và password khớp, nếu không tìm thấy trả về null
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        // Phương thức để lấy người dùng theo Id
        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
