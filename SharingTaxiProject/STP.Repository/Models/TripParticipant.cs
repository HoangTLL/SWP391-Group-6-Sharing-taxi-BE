namespace STP.Repository.Models
{
    public class TripParticipant
    {
        public int UserId { get; set; }
        public int TripId { get; set; }
        public decimal AmountPaid { get; set; }

        // Các thuộc tính bổ sung nếu cần
        public virtual Trip Trip { get; set; }
        public virtual User User { get; set; }
    }
}
