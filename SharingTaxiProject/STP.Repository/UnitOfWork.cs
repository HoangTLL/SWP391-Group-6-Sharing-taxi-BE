using STP.Repository.Models; // Tham chiếu đến các model
using STP.Repository.Repository; // Tham chiếu đến các repository như AreaRepository

namespace STP.Repository
{
    // Lớp UnitOfWork quản lý các repository và DbContext
    public class UnitOfWork : IDisposable
    {
        private readonly ShareTaxiContext _context;

        // Khai báo các repository
        private AreaRepository _areaRepository;
        private UserRepository _userRepository;
        private LocationRepository _locationRepository;
        private WalletRepository _walletRepository;
        private TransactionRepository _transactionRepository;
        private DepositRepository _depositRepository;
        private TripRepository _tripRepository;
        private TripTypeRepository _tripTypeRepository;
        private TripTypePricingRepository _tripTypePricingRepository;
        private BookingRepository _bookingRepository; // Thêm BookingRepository


        // Constructor không tham số: khởi tạo DbContext (ShareTaxiContext)
        public UnitOfWork() => _context = new ShareTaxiContext();

        // Constructor có tham số: nhận vào một đối tượng ShareTaxiContext
        public UnitOfWork(ShareTaxiContext context)
        {
            _context = context;
        }
        // Khởi tạo TripTypePricingRepository
        public TripTypePricingRepository TripTypePricingRepository =>
            _tripTypePricingRepository ??= new TripTypePricingRepository(_context);
        // Thuộc tính để truy cập TripRepository
        public TripRepository TripRepository =>
            _tripRepository ??= new TripRepository(_context);

        // Thuộc tính để truy cập AreaRepository. Sử dụng lazy loading (chỉ khởi tạo khi cần)
        public AreaRepository AreaRepository
        {
            get { return _areaRepository ??= new AreaRepository(_context); }
            // Nếu _areaRepository chưa được khởi tạo (null), khởi tạo mới với _context
        }

        // Thuộc tính để truy cập UserRepository. Sử dụng lazy loading
        public UserRepository UserRepository
        {
            get { return _userRepository ??= new UserRepository(_context); }
            // Nếu _userRepository chưa được khởi tạo (null), khởi tạo mới với _context
        }

        // Thuộc tính để truy cập LocationRepository. Sử dụng lazy loading
        public LocationRepository LocationRepository
        {
            get { return _locationRepository ??= new LocationRepository(_context); }
            // Nếu _locationRepository chưa được khởi tạo (null), khởi tạo mới với _context
        }
        public WalletRepository WalletRepository =>
        _walletRepository ??= new WalletRepository(_context);

        public TransactionRepository TransactionRepository =>
            _transactionRepository ??= new TransactionRepository(_context);

        public DepositRepository DepositRepository =>
            _depositRepository ??= new DepositRepository(_context);
        // Thuộc tính để truy cập TripTypeRepository
        public TripTypeRepository TripTypeRepository =>
            _tripTypeRepository ??= new TripTypeRepository(_context);
        // Thuộc tính để truy cập BookingRepository
        public BookingRepository BookingRepository
        {
            get { return _bookingRepository ??= new BookingRepository(_context); }
        }


        // Phương thức lưu thay đổi vào cơ sở dữ liệu
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Phương thức Dispose để giải phóng tài nguyên
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
