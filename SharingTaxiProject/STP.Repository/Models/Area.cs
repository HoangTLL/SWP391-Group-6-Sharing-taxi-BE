using System;
using System.Collections.Generic;

namespace STP.Repository.Models;

public partial class Area
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
