using Microsoft.AspNetCore.Mvc;
using STP.Repository;

[ApiController]
[Route("api/[controller]")]
public class FinanceController : ControllerBase
{
    private readonly WalletRepository _walletRepository;

    public FinanceController(WalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    // API: Tính tổng số tiền nạp vào
    [HttpGet("total-deposits")]
    public async Task<decimal> GetTotalDepositsAsync()
    {
        return await _walletRepository.GetTotalDepositsAsync(); // Sử dụng phương thức từ WalletRepository
    }

    // API: Tính doanh thu
    [HttpGet("total-revenue")]
    public async Task<ActionResult<decimal>> GetTotalRevenue()
    {
        var totalRevenue = await _walletRepository.GetTotalRevenueAsync();
        return Ok(totalRevenue);
    }

    // API: Top người dùng nạp tiền
    [HttpGet("top-depositors")]
    public async Task<List<object>> GetTopDepositorsAsync([FromQuery] int topCount)
    {
        return await _walletRepository.GetTopDepositorsAsync(topCount); // Sử dụng phương thức từ WalletRepository
    }
}
