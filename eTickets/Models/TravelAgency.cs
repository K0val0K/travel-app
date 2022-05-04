using eTickets.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Models
{
    public class TravelAgency:IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Travel agency Logo")]
        [Required(ErrorMessage = "Travel agency logo is required")]
        public string Logo { get; set; }

        [Display(Name = "Travel agency name")]
        [Required(ErrorMessage = "Travel agency name is required")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Travel agency description is required")]
        public string Description { get; set; }

        //Relationships
        public List<Tour> Tours { get; set; }
    }
}
