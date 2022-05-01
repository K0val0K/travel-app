using System.ComponentModel.DataAnnotations;

namespace eTickets.Models
{
    public class UserTourBookmark
    {
        [Key]
        public int Id { get; set; }

        public Movie Movie { get; set; }
        
        public ApplicationUser User { get; set; }
    }
}
