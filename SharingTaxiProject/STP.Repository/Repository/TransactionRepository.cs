using Microsoft.EntityFrameworkCore;
using PMS.Repository.Base; // Kế thừa từ GenericRepository
using STP.Repository.Models; // Tham chiếu đến model Transaction
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // Lấy tất cả các giao dịch liên quan đến chuyến đi dựa trên tripId và TransactionType
        public async Task<IEnumerable<Transaction>> GetTransactionsByTripIdAsync(int tripId, string transactionType)
        {
            return await _context.Transactions
                .Where(t => t.ReferenceId.Contains($"Trip_{tripId}") && t.TransactionType == transactionType)
                .ToListAsync();
        }
    }
}
