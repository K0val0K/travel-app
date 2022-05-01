using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public class BookmarksService : IBookmarksService
    {
        private readonly AppDbContext _context;

        public BookmarksService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task AddUserBookmark(string userId, Movie movie)
        {
            var user = await _context.Users.FindAsync(userId);

            var bookmark = new UserTourBookmark()
            {
                User = user,
                Movie = movie,
            };

            if(user != null && !_context.Bookmarks.Any(x => x.User == user && x.Movie == movie))
            {
                await _context.AddAsync(bookmark);
                await _context.SaveChangesAsync();
            }       
        }

        public async Task DeleteUserBookmark(string userId, Movie movie)
        {
            var user = _context.Users.Find(userId);
            var bookmark = await _context.Bookmarks.FirstOrDefaultAsync(x => x.Movie == movie && x.User == user);

            _context.Bookmarks.Remove(bookmark);
            _context.SaveChanges();
        }

        public async Task<List<UserTourBookmark>> GetUserBookmarksAsync(string userId)
        {
            var bookmarks = await _context.Bookmarks.Include(n => n.User).Include(n => n.Movie).ToListAsync();

            bookmarks = bookmarks.Where(n => n.User.Id == userId).ToList();

            return bookmarks;
        }
    }
}
