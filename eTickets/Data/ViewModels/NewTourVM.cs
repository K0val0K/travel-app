using eTickets.Data;
using eTickets.Data.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Models
{
    public class NewTourVM
    {
        public int Id { get; set; }

        [Display(Name = "Tour name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "Tour description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Display(Name = "Price in $")]
        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        public string ImageURL  { get; set; }

        [Display(Name = "Tour poster URL")]
        public IFormFile Image { get; set; }

        [Display(Name = "Tour start date")]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Tour end date")]
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Select a category")]
        [Required(ErrorMessage = "Tour category is required")]
        public TourCategory TourCategory { get; set; }

        //Relationships
        [Display(Name = "Select country(-es)")]
        [Required(ErrorMessage = "Tour country(-es) is required")]
        public List<int> CountryIds { get; set; }

        [Display(Name = "Select a travel agency")]
        [Required(ErrorMessage = "Travel agency is required")]
        public int TravelAgencyId { get; set; }
    }
}
