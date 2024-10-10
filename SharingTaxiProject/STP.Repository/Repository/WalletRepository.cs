using Microsoft.EntityFrameworkCore;
using PMS.Repository.Base; // Kế thừa từ GenericRepository
using STP.Repository.Models; // Tham chiếu đến model Wallet

namespace STP.Repository
{
    public class WalletRepository : GenericRepository<Wallet>
    {
        public WalletRepository(ShareTaxiContext context) : base(context) { }

        // Lấy ví điện tử theo userId
        public async Task<Wallet> GetWalletByUserIdAsync(int userId)
        {
            return await _context.Wallets
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }
    }
}
