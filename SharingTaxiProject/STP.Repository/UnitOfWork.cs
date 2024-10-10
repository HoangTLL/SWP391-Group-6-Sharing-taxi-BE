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
<<<<<<< Updated upstream
        private AreaRepository _areaRepository;
        private UserRepository _userRepository;
        public UnitOfWork() => _context = new ShareTaxiContext();
        public UnitOfWork(ShareTaxiContext context) { _context = context; }
        public AreaRepository areaRepository
        {
            get { return _areaRepository ??= new AreaRepository(_context); }
=======

        // Khai báo các repository
        //private AreaRepository _areaRepository;
        private UserRepository _userRepository;
        private LocationRepository _locationRepository;
        private TripRepository _tripRepository;
        private TripTypeRepository _tripTypeRepository;

        // Constructor không tham số: khởi tạo DbContext (ShareTaxiContext)
        public UnitOfWork() => _context = new ShareTaxiContext();

        // Constructor có tham số: nhận vào một đối tượng ShareTaxiContext
        public UnitOfWork(ShareTaxiContext context)
        {
            _context = context;
        }

        private AreaRepository _areaRepository; // Field to hold the instance of AreaRepository

        // Property to access AreaRepository using lazy loading
        public AreaRepository AreaRepository
        {
            get
            {
                // If _areaRepository is null, initialize it with the current context
                return _areaRepository ??= new AreaRepository(_context);
            }
>>>>>>> Stashed changes
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
        // Thuộc tính để truy cập TripRepository. Sử dụng lazy loading
        public TripRepository TripRepository
        {
            get { return _tripRepository ??= new TripRepository(_context); }
            // Nếu _tripRepository chưa được khởi tạo (null), khởi tạo mới với _context
        }
        // Thuộc tính để truy cập TripTypeRepository. Sử dụng lazy loading
        public TripTypeRepository TripTypeRepository
        {
            get { return _tripTypeRepository ??= new TripTypeRepository(_context); }
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
>>>>>>> Stashed changes
        }
    }
}
