﻿using System;
using System.Collections.Generic;

namespace STP.Repository.Models;

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

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<CarTrip> CarTrips { get; set; } = new List<CarTrip>();

    public virtual Location? DropOffLocation { get; set; }

    public virtual Location? PickUpLocation { get; set; }

    public virtual Pricing? Pricing { get; set; }

    public virtual Area? ToArea { get; set; }
}
