using eTickets.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public interface IBookmarksService
    {
        Task<List<UserTourBookmark>> GetUserBookmarksAsync(string userId);

        Task AddUserBookmark(string userId, Movie movie);

        Task DeleteUserBookmark(string userId, Movie movie);
    }
}
