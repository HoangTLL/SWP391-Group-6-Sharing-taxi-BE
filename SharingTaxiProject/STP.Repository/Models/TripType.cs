using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Repository.Models
{
    public class TripType
    {
        public int Id { get; set; }
        public int FromAreaId { get; set; }
        public int ToAreaId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public decimal? BasicePrice { get; set; }

        // Navigation Properties
        public virtual ICollection<TripeTypePricing> TripeTypePricings { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }



    }
}
