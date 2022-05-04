using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.ViewModels
{
    public class OrderVM
    {
        public string TourName { get; set; }
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        public string ContactEmail { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }

        [Required]
        public int PersonQuantity { get; set; }
    }
}
