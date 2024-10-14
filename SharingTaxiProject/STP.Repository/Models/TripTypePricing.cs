using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Repository.Models
{
    public class TripTypePricing
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TripType { get; set; }
        public int MinPerson { get; set; }
        public int MaxPerson { get; set; }
        public decimal PricePerPerson { get; set; }

        // Navigation Property
        public virtual TripType TripTypeNavigation { get; set; }
    }
}
