using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCCore.Data;
using MVCCore.Models;
using MVCCore.Models.Enumerations;
using MVCCore.Services;

namespace MVCCore.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("manage")]
    public class GameController : Controller
    {
        private readonly PlayscoreDbContext _context;

        public GameController(PlayscoreDbContext context)
        {
            _context = context;
        }

        // GET: manage/game
        [HttpGet("")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5)
        {
            var games = _context.Games;
            var gamePage = await PaginatedList<GameModel>.CreateAsync(games, pageIndex, pageSize);
            return _context.Games != null ?
                View(gamePage) :
                Problem("There are no games in the records.");
        }

        // GET: manage/game/details/{id}
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var gameModel = await _context.Games
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameModel == null)
            {
                return NotFound();
            }

            return View(gameModel);
        }

        // GET: manage/game/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: manage/game/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Rating,Category,AgeRating")] GameModel gameModel)
        {
            if (ModelState.IsValid)
            {
                gameModel.Id = Guid.NewGuid();
                _context.Add(gameModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gameModel);
        }

        // GET: manage/game/edit/{id}
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var gameModel = await _context.Games.FindAsync(id);
            if (gameModel == null)
            {
                return NotFound();
            }
            return View(gameModel);
        }

        // POST: manage/game/edit/{id}
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,Rating,Category,AgeRating")] GameModel gameModel)
        {
            if (id != gameModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameModelExists(gameModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gameModel);
        }

        // GET: manage/game/delete/{id}
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var gameModel = await _context.Games
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameModel == null)
            {
                return NotFound();
            }

            return View(gameModel);
        }

        // POST: manage/game/delete/{id}
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Games == null)
            {
                return Problem("Entity set 'PlayscoreDbContext.Games' is null.");
            }
            var gameModel = await _context.Games.FindAsync(id);
            if (gameModel != null)
            {
                _context.Games.Remove(gameModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameModelExists(Guid id)
        {
            return (_context.Games?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
