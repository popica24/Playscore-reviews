using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCCore.Data;
using MVCCore.Models;
using MVCCore.Services;

namespace MVCCore.Controllers
{
    [Route("{gameId}")]

    public class ReviewController : Controller
    {
        private readonly PlayscoreDbContext _context;
        private readonly ILogger<HomeController> _logger;
        public ReviewController(ILogger<HomeController> logger, PlayscoreDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("details")]
        public async Task<IActionResult> Index(Guid gameId, int pageIndex = 1, int pageSize = 3)
        {
            var game = await _context.Games.FirstOrDefaultAsync(x => x.Id == gameId);
            var reviews = _context.Reviews.Where(x => x.GameId == gameId);
            game.Reviews = await PaginatedList<ReviewModel>.CreateAsync(reviews, pageIndex, pageSize);
            try
            {
                game.Rating = await reviews.SumAsync(x => x.Stars) / await reviews.CountAsync();
                await _context.SaveChangesAsync();
            }
            catch (DivideByZeroException)
            {
                game.Rating = 0;
            }
            return View(game);
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> OnPostCreateReview(ReviewModel request)
        {
            await _context.Reviews.AddAsync(request);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Information, ex.Message);
                ViewData["ValidateMessage"] = "Already reviewed the game !";
            }
            return RedirectToAction("Index", "Review", new { gameId = request.GameId });
        }

        [Authorize]
        [HttpPost]
        [Route("review")]
        public async Task<IActionResult> OnDeleteReview(Guid requestId)
        {
            var request = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == requestId);
            _context.Reviews.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Review", new { gameId = request.GameId });
        }

    }
}
