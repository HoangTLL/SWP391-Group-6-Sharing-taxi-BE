using Microsoft.EntityFrameworkCore;
using PMS.Repository.Base; // Kế thừa từ GenericRepository
using STP.Repository.Models; // Tham chiếu đến model Transaction

namespace STP.Repository
{
    public class TransactionRepository : GenericRepository<Transaction>
    {
        public TransactionRepository(ShareTaxiContext context) : base(context) { }

        // Lấy tất cả các giao dịch của ví
        public async Task<IEnumerable<Transaction>> GetByWalletIdAsync(int walletId)
        {
            return await _context.Transactions
                .Where(t => t.WalletId == walletId)
                .ToListAsync();
        }
    }
}
