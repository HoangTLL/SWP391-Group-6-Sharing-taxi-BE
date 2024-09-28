using System;
using System.Collections.Generic;

namespace STP.Repository.Models;

public partial class Location
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Lat { get; set; }

    public decimal? Lon { get; set; }

    public int? AreaId { get; set; }

    public virtual Area? Area { get; set; }

    public virtual ICollection<Pricing> PricingDropOffLocations { get; set; } = new List<Pricing>();

    public virtual ICollection<Pricing> PricingPickUpLocations { get; set; } = new List<Pricing>();

    public virtual ICollection<Trip> TripDropOffLocations { get; set; } = new List<Trip>();

    public virtual ICollection<Trip> TripPickUpLocations { get; set; } = new List<Trip>();
}
