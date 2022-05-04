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
        public async Task AddUserBookmark(string userId, Tour tour)
        {
            var user = await _context.Users.FindAsync(userId);

            var bookmark = new UserTourBookmark()
            {
                User = user,
                Tour = tour,
            };

            if(user != null && !_context.Bookmarks.Any(x => x.User == user && x.Tour == tour))
            {
                await _context.AddAsync(bookmark);
                await _context.SaveChangesAsync();
            }       
        }

        public async Task DeleteTourBookmarks(Tour tour)
        {
            var bookmarks = await _context.Bookmarks.Where(x => x.Tour == tour).ToListAsync();

            foreach(var bookmark in bookmarks)
            {
                _context.Bookmarks.Remove(bookmark);
            }

            _context.SaveChanges();
        }

        public async Task DeleteUserBookmark(string userId, Tour tour)
        {
            var user = await _context.Users.FindAsync(userId);

            var bookmark = await _context.Bookmarks.FirstOrDefaultAsync(x => x.Tour == tour && x.User == user);

            if(bookmark != null)
            {
                _context.Bookmarks.Remove(bookmark);
            }
            _context.SaveChanges();
        }


        public async Task<List<UserTourBookmark>> GetUserBookmarksAsync(string userId)
        {
            var bookmarks = await _context.Bookmarks.Include(n => n.User).Include(n => n.Tour).ToListAsync();

            bookmarks = bookmarks.Where(n => n.User.Id == userId).ToList();

            return bookmarks;
        }
    }
}
