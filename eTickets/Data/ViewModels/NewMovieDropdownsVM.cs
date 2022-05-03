using eTickets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.ViewModels
{
    public class NewMovieDropdownsVM
    {
        public NewMovieDropdownsVM()
        {
            Producers = new List<Producer>();
            TravelAgencies = new List<TravelAgency>();
            Actors = new List<Actor>();
        }

        public List<Producer> Producers { get; set; }
        public List<TravelAgency> TravelAgencies { get; set; }
        public List<Actor> Actors { get; set; }
    }
}
