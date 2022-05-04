using eTickets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.ViewModels
{
    public class NewTourDropdownsVM
    {
        public NewTourDropdownsVM()
        {
            TravelAgencies = new List<TravelAgency>();
            Countries = new List<Country>();
        }

        public List<TravelAgency> TravelAgencies { get; set; }
        public List<Country> Countries { get; set; }
    }
}
