using STP.Repository.Models;
using STP.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Repository
{
    public class UnitOfWork
    {
        private readonly ShareTaxiContext _context;
        private AreaRepository _areaRepository;
        private UserRepository _userRepository;
<<<<<<< Updated upstream
=======
        private LocationRepository _locationRepository;
        private WalletRepository _walletRepository;
        private TransactionRepository _transactionRepository;
        private DepositRepository _depositRepository;
        // Constructor không tham số: khởi tạo DbContext (ShareTaxiContext)
>>>>>>> Stashed changes
        public UnitOfWork() => _context = new ShareTaxiContext();
        public UnitOfWork(ShareTaxiContext context) { _context = context; }
        public AreaRepository areaRepository
        {
            get { return _areaRepository ??= new AreaRepository(_context); }
        }
        public UserRepository UserRepository
        {
            get { return _userRepository ??= new UserRepository(_context); }
<<<<<<< Updated upstream
=======
            // Nếu _userRepository chưa được khởi tạo (null), khởi tạo mới với _context
        }

        // Thuộc tính để truy cập LocationRepository. Sử dụng lazy loading
        public LocationRepository LocationRepository
        {
            get { return _locationRepository ??= new LocationRepository(_context); }
            // Nếu _locationRepository chưa được khởi tạo (null), khởi tạo mới với _context
        }
        public TransactionRepository TransactionRepository =>
        _transactionRepository ??= new TransactionRepository(_context);

        public DepositRepository DepositRepository =>
            _depositRepository ??= new DepositRepository(_context);
        // Phương thức lưu thay đổi vào cơ sở dữ liệu
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public WalletRepository WalletRepository =>
        _walletRepository ??= new WalletRepository(_context);

        // Phương thức Dispose để giải phóng tài nguyên
        public void Dispose()
        {
            _context.Dispose();
>>>>>>> Stashed changes
        }
    }
}
