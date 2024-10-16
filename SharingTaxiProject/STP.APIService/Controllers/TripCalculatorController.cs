using Microsoft.AspNetCore.Mvc;
using STP.Repository.Models;

[ApiController]
[Route("api/[controller]")]
public class TripCalculatorController : ControllerBase
{
    private readonly ShareTaxiContext _context;

    public TripCalculatorController(ShareTaxiContext context)
    {
        _context = context;
    }

    // POST: api/TripCalculator/CalculateTotalCost
    [HttpPost("CalculateTotalCost")]
    public IActionResult CalculateTotalCost(int fromAreaId, int toAreaId, int minPerson, int maxPerson)
    {
        // Kiểm tra điều kiện: MinPerson không được lớn hơn MaxPerson
        if (minPerson > maxPerson)
        {
            return BadRequest("MinPerson cannot be greater than MaxPerson.");
        }

        // Kiểm tra điều kiện: fromAreaId không được trùng với toAreaId
        if (fromAreaId == toAreaId)
        {
            return BadRequest("From and To areas cannot be the same.");
        }

        // Bước 1: Tìm thông tin về TripType dựa trên fromAreaId và toAreaId
        var tripType = _context.TripTypes
            .FirstOrDefault(tt => tt.FromAreaId == fromAreaId && tt.ToAreaId == toAreaId);

        if (tripType == null)
        {
            return NotFound("Trip type not found for the specified areas.");
        }

        // Bước 2: Lấy thông tin giá từ bảng TripeTypePricing dựa trên minPerson mà người dùng chọn
        var pricing = _context.TripTypePricings
            .FirstOrDefault(p => p.TripType == tripType.Id && p.MinPerson == minPerson);

        if (pricing == null)
        {
            return NotFound("Pricing not available for the specified minPerson.");
        }

        // Bước 3: Lấy giá từ TripeTypePricing cho mỗi người
        decimal pricePerPerson = pricing.PricePerPerson;

        // Bước 4: Tính tổng chi phí dựa trên giá của MinPerson nhưng số lượng người là MaxPerson
        decimal totalCost = pricePerPerson * maxPerson;  // Tổng giá theo số lượng người MaxPerson, nhưng giá là của MinPerson

        // Bước 5: Trả về kết quả bao gồm MinPerson và MaxPerson và tổng chi phí
        return Ok(new
        {
            id = tripType.Id,
            from = fromAreaId,
            to = toAreaId,
            minPerson = minPerson,       // Thông tin MinPerson mà người dùng chọn
            maxPerson = maxPerson,       // Thông tin MaxPerson mà người dùng chọn
            pricePerPerson = pricePerPerson,  // Giá mỗi người dựa trên MinPerson
            totalCost = totalCost        // Tổng chi phí dựa trên MaxPerson
        }); 
    }

}
