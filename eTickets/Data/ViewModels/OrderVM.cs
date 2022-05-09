using eTickets.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public string TourName { get; set; }
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        public string ContactEmail { get; set; }

        [Display(Name = "Contant Phone")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }

        [Display(Name = "Person Quantity")]
        [Required]
        public int PersonQuantity { get; set; }

        public string Comment { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
