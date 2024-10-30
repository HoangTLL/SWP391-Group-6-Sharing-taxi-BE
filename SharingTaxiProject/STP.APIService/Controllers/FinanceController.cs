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

    /// <summary>
    /// API to calculate the total amount of deposits.
    /// </summary>
    /// <returns>Total deposits as a decimal value.</returns>
    [HttpGet("total-deposits")]
    public async Task<decimal> GetTotalDepositsAsync()
    {
        // BƯỚC 1: Lấy tổng số tiền nạp từ WalletRepository
        return await _walletRepository.GetTotalDepositsAsync();
    }

    /// <summary>
    /// API to calculate total revenue.
    /// </summary>
    /// <returns>Total revenue as a decimal value.</returns>
    [HttpGet("total-revenue")]
    public async Task<ActionResult<decimal>> GetTotalRevenue()
    {
        // BƯỚC 1: Lấy tổng doanh thu từ WalletRepository
        var totalRevenue = await _walletRepository.GetTotalRevenueAsync();

        // BƯỚC 2: Trả về tổng doanh thu
        return Ok(totalRevenue);
    }

    /// <summary>
    /// API to retrieve the top depositors based on deposit amount.
    /// </summary>
    /// <param name="topCount">The number of top depositors to retrieve.</param>
    /// <returns>List of top depositors with details.</returns>
    [HttpGet("top-depositors")]
    public async Task<List<object>> GetTopDepositorsAsync([FromQuery] int topCount)
    {
        // BƯỚC 1: Lấy danh sách người nạp tiền hàng đầu từ WalletRepository
        return await _walletRepository.GetTopDepositorsAsync(topCount);
    }
}
