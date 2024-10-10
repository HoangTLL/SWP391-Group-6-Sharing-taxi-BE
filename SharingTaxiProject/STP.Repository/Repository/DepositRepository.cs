using PMS.Repository.Base;
using STP.Repository.Models;

namespace STP.Repository
{
    public class DepositRepository : GenericRepository<Deposit>
    {
        public DepositRepository(ShareTaxiContext context) : base(context) { }
    }
}
