﻿using eTickets.Data;
using eTickets.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Models
{
    public class Tour:IEntityBase
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageURL { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TourCategory TourCategory { get; set; }

        //Relationships
        public List<Country_Tour> Countries_Tours { get; set; }

        //TravelAgency
        public int TravelAgencyId { get; set; }
        [ForeignKey("TravelAgencyId")]
        public TravelAgency TravelAgency { get; set; }
    }
}
