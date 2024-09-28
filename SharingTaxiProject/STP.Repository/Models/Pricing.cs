using System;
using System.Collections.Generic;

namespace STP.Repository.Models;

public partial class Pricing
{
    public int Id { get; set; }

    public int? PickUpLocationId { get; set; }

    public int? DropOffLocationId { get; set; }

    public decimal? Price { get; set; }

    public string? Currency { get; set; }

    public int? Status { get; set; }

    public virtual Location? DropOffLocation { get; set; }

    public virtual Location? PickUpLocation { get; set; }

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
