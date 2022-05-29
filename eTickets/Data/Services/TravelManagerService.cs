using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public class TravelManagerService : ITravelManagerService
    {
        private readonly AppDbContext _context;
        public TravelManagerService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetTravelAgencyIdByManagerUserId(string userId)
        {
            return _context.AgencyManagers.FirstOrDefault(x => x.UserId == userId).TravelAgencyId;
        }
    }
}
