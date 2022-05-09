using eTickets.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Models
{
    public class OrderHistoryItem
    {
        [Key]
        public int Id { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string Comment { get; set; }

        public DateTime CreateDate { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}
