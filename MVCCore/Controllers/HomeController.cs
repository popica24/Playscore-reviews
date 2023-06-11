using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCCore.Data;
using MVCCore.Models;
using MVCCore.Services;
using System.Diagnostics;
using System.Security.Claims;

namespace MVCCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PlayscoreDbContext _context;
   

        public HomeController(ILogger<HomeController> logger,PlayscoreDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("")]
        [Route("{searchName}")]
        public async Task<IActionResult> Index(string? searchName = null,int pageIndex = 1,int pageSize = 6)
        {
            if (string.IsNullOrEmpty(searchName))
            {
                var gamePage = await PaginatedList<GameModel>.CreateAsync(_context.Games, pageIndex, pageSize);
                return View(gamePage);
            }
            var page = await PaginatedList<GameModel>.CreateAsync(_context.Games.Where(g => g.Name.Contains(searchName)), pageIndex, pageSize);
            return View(page);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}