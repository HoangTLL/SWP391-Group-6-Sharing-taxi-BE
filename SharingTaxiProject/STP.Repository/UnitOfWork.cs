using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using STP.Repository.Models;
using STP.Repository.Repository;

namespace STP.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly ShareTaxiContext _context;
        private readonly ILogger _logger;
        private IDbContextTransaction _transaction;

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
        private BookingRepository _bookingRepository;
        private CarTripRepository _carTripRepository;

        // Constructor
        public UnitOfWork(ShareTaxiContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Repository properties
        public AreaRepository AreaRepository =>
            _areaRepository ??= new AreaRepository(_context);

        public UserRepository UserRepository =>
            _userRepository ??= new UserRepository(_context);

        public LocationRepository LocationRepository =>
            _locationRepository ??= new LocationRepository(_context);

        public WalletRepository WalletRepository =>
            _walletRepository ??= new WalletRepository(_context);

        public TransactionRepository TransactionRepository =>
            _transactionRepository ??= new TransactionRepository(_context);

        public DepositRepository DepositRepository =>
            _depositRepository ??= new DepositRepository(_context);

        public TripRepository TripRepository =>
            _tripRepository ??= new TripRepository(_context);

        public TripTypeRepository TripTypeRepository =>
            _tripTypeRepository ??= new TripTypeRepository(_context);

        public TripTypePricingRepository TripTypePricingRepository =>
            _tripTypePricingRepository ??= new TripTypePricingRepository(_context, _logger);

        public BookingRepository BookingRepository =>
            _bookingRepository ??= new BookingRepository(_context);

        public CarTripRepository CarTripRepository =>
            _carTripRepository ??= new CarTripRepository(_context);

        // Transaction methods
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            }
            return _transaction;
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                await _transaction?.RollbackAsync();
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        // Save changes
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Dispose
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}