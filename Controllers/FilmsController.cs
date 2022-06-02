using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba2FilmsBD.Models;

namespace Laba2FilmsBD.Controllers
{
    public class FilmsController : Controller
    {
        private readonly LabaFilmsDBContext _context;
        private readonly static RequestsController request = new RequestsController();

        public FilmsController(LabaFilmsDBContext context)
        {
            _context = context;
        }

        // GET: Films
        public async Task<IActionResult> Index(int? Id, string? val, int? role, int? min, int? max, int? Rnumber)
        {

            ViewBag.contextActors = _context.Actors.ToList();
            ViewBag.Genres = _context.Films.Select(x => x.Genre).Distinct().ToList();

            ViewBag.LastRequest = null;
            ViewBag.LastSearch = null;          
            ViewBag.ErrorMessage = null;
            if (Rnumber == null) return View(await _context.Films.ToListAsync());
            if (Rnumber == 4) 
            {
                ViewBag.LastRequest = "4";
                ViewBag.LastSearch = Id;
                ViewBag.LastValue = val;
                if (role == 0) ViewBag.Role = "";
                if (role == 1) ViewBag.Role = "у головній ролі";
                if (role == 2) ViewBag.Role = "у другорядній ролі";
                return View(request.Request4((int)Id, (string)val, (int)role));
            }
            if (Rnumber == 5)
            {
                ViewBag.LastRequest = "5";
                ViewBag.LastSearch = Id;
                ViewBag.LastMin = min;
                ViewBag.LastMax = max;
                if (min == null || min < 0 || max == null || max < 0)
                {
                    ViewBag.ErrorMassage = "Обмеження тривалості мають бути >= 0";
                    ViewBag.LastMin = null;
                    ViewBag.LastMax = null;
                    return View(await _context.Films.ToListAsync());
                }
                return View(request.Request5((int)Id, (int)min, (int)max));
            }
            ViewBag.ErrorMassage = "Unknown request";

            return View(await _context.Films.ToListAsync());
         
        }

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: Films/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Films/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,ReleaseYear,Language,LenghtInMinutes,Genre")] Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ReleaseYear,Language,LenghtInMinutes,Genre")] Film film)
        {
            if (id != film.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(film);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.Id))
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
            return View(film);
        }

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Films == null)
            {
                return Problem("Entity set 'LabaFilmsDBContext.Films'  is null.");
            }
            var film = await _context.Films.FindAsync(id);
            if (film != null)
            {
                _context.Films.Remove(film);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
          return (_context.Films?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public bool NameNotExists(string Title)
        {
            var find = _context.Films.FirstOrDefault(x => x.Title == Title);
            if (find == null) return true;
            else return false;
        }
    }
}
