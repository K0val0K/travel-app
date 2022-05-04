using System.ComponentModel.DataAnnotations;

namespace eTickets.Models
{
    public class UserTourBookmark
    {
        [Key]
        public int Id { get; set; }

        public Tour Tour { get; set; }
        
        public ApplicationUser User { get; set; }
    }
}
