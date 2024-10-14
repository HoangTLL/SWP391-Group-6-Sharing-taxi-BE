using System;
using System.Collections.Generic;

namespace STP.Repository.Models;

public partial class CarTrip
{
    public int Id { get; set; }

    public int? TripId { get; set; }

    public string? DriverName { get; set; }

    public string? DriverPhone { get; set; }

    public string? PlateNumber { get; set; }

    public TimeOnly? ArrivedTime { get; set; }

    public int? Status { get; set; }

    public virtual Trip? Trip { get; set; }
}
