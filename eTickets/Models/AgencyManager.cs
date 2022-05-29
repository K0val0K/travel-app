using eTickets.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Models
{
    public class AgencyManager:IEntityBase
    {
        [Key]
        public int Id { get; set; }

        public int TravelAgencyId { get; set; }
        [ForeignKey("TravelAgencyId")]
        public TravelAgency TravelAgency { get; set; }

        public string UserId { get;  set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
