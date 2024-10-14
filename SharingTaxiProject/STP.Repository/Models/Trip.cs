using System;
using System.Collections.Generic;

namespace STP.Repository.Models
{
    public partial class Trip
    {
        public int Id { get; set; }

        public int? PickUpLocationId { get; set; }

        public int? DropOffLocationId { get; set; }

        public int? ToAreaId { get; set; }

        public int? MaxPerson { get; set; }

        public int? MinPerson { get; set; }

        public decimal? UnitPrice { get; set; }

        public DateTime? BookingDate { get; set; }

        public TimeOnly? HourInDay { get; set; }

        public int? PricingId { get; set; }

        // Khóa ngoại liên kết đến TripType
        public int TripTypeId { get; set; }

        // Collection chứa các đối tượng Booking liên kết với Trip
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        // Collection chứa các đối tượng CarTrip liên kết với Trip
        public virtual ICollection<CarTrip> CarTrips { get; set; } = new List<CarTrip>();

        // Đối tượng DropOffLocation (điểm đến)
        public virtual Location? DropOffLocation { get; set; }

        // Đối tượng PickUpLocation (điểm đón)
        public virtual Location? PickUpLocation { get; set; }

        // Đối tượng ToArea (khu vực đến)
        public virtual Area? ToArea { get; set; }

        // Đối tượng TripType (kiểu chuyến đi)
        public virtual TripType? TripType { get; set; }  // Thêm thuộc tính điều hướng đến TripType
    }
}
