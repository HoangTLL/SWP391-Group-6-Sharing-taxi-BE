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

        // Tính tổng số tiền nạp vào
        public async Task<decimal> GetTotalDepositsAsync()
        {
            return await _context.Transactions
                .Where(t => t.TransactionType == "Deposit")
                .SumAsync(t => (decimal?)t.Amount) ?? 0; // Chuyển đổi sang decimal? và cung cấp giá trị mặc định 0
        }

        // Tính doanh thu
        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Transactions
                .Where(t => t.TransactionType == "Payment")
                .SumAsync(t => (decimal?)t.Amount) ?? 0; // Chuyển đổi sang decimal? và cung cấp giá trị mặc định 0
        }


        // Lấy danh sách top người dùng nạp vào
        public async Task<List<object>> GetTopDepositorsAsync(int topCount)
        {
            return await _context.Transactions
                .Where(t => t.TransactionType == "Deposit")
                .GroupBy(t => t.Wallet.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    TotalDeposit = g.Sum(t => t.Amount)
                })
                .OrderByDescending(g => g.TotalDeposit)
                .Take(topCount)
                .ToListAsync<object>();
        }
    }
}
