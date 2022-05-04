using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public int PersonQuantity { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public List<TourOrderHistory> OrderHistoryItems { get; set; }
    }
}
