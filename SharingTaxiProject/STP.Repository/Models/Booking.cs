﻿using System;
using System.Collections.Generic;

namespace STP.Repository.Models;

public partial class Booking
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? TripId { get; set; }

    public virtual Trip? Trip { get; set; }

    public virtual User? User { get; set; }
}
