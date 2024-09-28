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
        public UnitOfWork() => _context = new ShareTaxiContext();
        public UnitOfWork(ShareTaxiContext context) { _context = context; }
        public AreaRepository areaRepository
        {
            get { return _areaRepository ??= new AreaRepository(_context); }
        }
        public UserRepository UserRepository
        {
            get { return _userRepository ??= new UserRepository(_context); }
        }
    }
}
