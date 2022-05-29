using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public interface ITravelManagerService
    {
        Task<int> GetTravelAgencyIdByManagerUserId(string userId);
    }
}
