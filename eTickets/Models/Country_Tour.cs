using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Models
{
    public class Country_Tour
    {
        public int TourId { get; set; }
        public Tour Tour { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
