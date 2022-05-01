using eTickets.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    [Authorize]
    public class BookmarksController : Controller
    {
        private readonly IBookmarksService _bookmarksService;
        private readonly IMoviesService _moviesService;
        public BookmarksController(IBookmarksService bookmarksService, IMoviesService moviesService)
        {
            _bookmarksService = bookmarksService;
            _moviesService = moviesService;
        }
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookmarks = await _bookmarksService.GetUserBookmarksAsync(userId);
            return View(bookmarks);
        }

        public async Task<IActionResult> AddUserBookmark(int id)
        {
            var item = await _moviesService.GetByIdAsync(id);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            

            if (item != null)
            {
               await  _bookmarksService.AddUserBookmark(userId, item);
            }

            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> RemoveUserBookMark(int id)
        //{
            
        //}
    }
}
