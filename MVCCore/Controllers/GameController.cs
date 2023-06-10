using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCCore.Data;
using MVCCore.Models;
using MVCCore.Services;

namespace MVCCore.Controllers
{
    [Route("{gameId}/")]
    
    public class GameController : Controller
    {
        private readonly PlayscoreDbContext _context;
        private readonly ILogger<HomeController> _logger;
        public GameController(ILogger<HomeController> logger, PlayscoreDbContext context)
        {
            _logger = logger;   
            _context = context;
        }

        [Route("")]
        public async Task<IActionResult> Index(Guid gameId, int pageIndex = 1, int pageSize = 3)
        {
            var game = await _context.Games.FirstOrDefaultAsync(x=>x.Id == gameId);
            var reviews = _context.Reviews.Where(x => x.GameId == gameId);
            game.Reviews = await PaginatedList<ReviewModel>.CreateAsync(reviews,pageIndex, pageSize);
            try
            {
                game.Rating = await reviews.SumAsync(x => x.Stars) / await reviews.CountAsync();
            }
            catch(DivideByZeroException)
            {
                game.Rating = 0;
            }
            return View(game);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> OnPostCreateReview(ReviewModel request)
        {

            if(request==null)
            {
                return RedirectToAction("Index", "Game", new { gameId = request.GameId });
            }
            await _context.Reviews.AddAsync(request);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Information, ex.Message);
                ViewData["ValidateMessage"] = "Already reviewed the game !";
            }
            return RedirectToAction("Index","Game", new { gameId = request.GameId });
        }
    }
}
