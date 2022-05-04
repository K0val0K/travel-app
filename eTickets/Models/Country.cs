using eTickets.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Models
{
    public class Country:IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage = "Profile Picture is required")]
        public string CountryPictureURL { get; set; }

        [Display(Name = "Country Name")]
        [Required(ErrorMessage = "Country Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Country Name must be between 2 and 50 chars")]
        public string CountryName { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        //Relationships
        public List<Country_Tour> Countries_Tours { get; set; }
    }
}
