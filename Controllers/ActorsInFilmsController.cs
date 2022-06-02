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
    public class ActorsInFilmsController : Controller
    {
        private readonly LabaFilmsDBContext _context;

        public ActorsInFilmsController(LabaFilmsDBContext context)
        {
            _context = context;
        }

        // GET: ActorsInFilms
        public async Task<IActionResult> Index()
        {
            var labaFilmsDBContext = _context.ActorsInFilms.Include(a => a.Actor).Include(a => a.Film);
            return View(await labaFilmsDBContext.ToListAsync());
        }        

        // GET: ActorsInFilms/Create
        public IActionResult Create()
        {
            ViewBag.contextActors = _context.Actors;
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Title");
            return View();
        }

        // POST: ActorsInFilms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActorId,FilmId,Сharacter,IsMain")] ActorsInFilm actorsInFilm)
        {
            ViewBag.ErrorMessage = "";
            if (NotExists(actorsInFilm.ActorId, actorsInFilm.FilmId))
            {
                _context.Add(actorsInFilm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ErrorMessage = "Такий запис вже існує";
            ViewBag.contextActors = _context.Actors;
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Title", actorsInFilm.FilmId);
            return View(actorsInFilm);
        }

        // GET: ActorsInFilms/Edit/5
        public async Task<IActionResult> Edit(int? ActorId, int? FilmId)
        {
            if (ActorId == null || FilmId == null || _context.ActorsInFilms == null)
            {
                return NotFound();
            }

            var actorsInFilm = await _context.ActorsInFilms.FirstOrDefaultAsync(x => (x.ActorId == ActorId && x.FilmId == FilmId));
            if (actorsInFilm == null)
            {
                return NotFound();
            }
            actorsInFilm.Actor = await _context.Actors.FirstAsync(x => x.Id == actorsInFilm.ActorId);
            actorsInFilm.Film = await _context.Films.FirstAsync(x => x.Id == actorsInFilm.FilmId);
            return View(actorsInFilm);
        }

        // POST: ActorsInFilms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int ActorId, int FilmId, [Bind("ActorId,FilmId,Сharacter,IsMain")] ActorsInFilm actorsInFilm)
        {
            if (ActorId != actorsInFilm.ActorId || FilmId != actorsInFilm.FilmId)
            {
                return NotFound();
            }
            
          //  if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actorsInFilm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorsInFilmExists(actorsInFilm.ActorId, actorsInFilm.FilmId))
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
            actorsInFilm.Actor = await _context.Actors.FirstAsync(x => x.Id == actorsInFilm.ActorId);
            actorsInFilm.Film = await _context.Films.FirstAsync(x => x.Id == actorsInFilm.FilmId);
            return View(actorsInFilm);
        }

        // GET: ActorsInFilms/Delete/5
        public async Task<IActionResult> Delete(int? ActorId, int? FilmId)
        {
            if (ActorId == null || FilmId == null || _context.ActorsInFilms == null)
            {
                return NotFound();
            }

            var actorsInFilm = await _context.ActorsInFilms
                .Include(a => a.Actor)
                .Include(a => a.Film)
                .FirstOrDefaultAsync(m => m.ActorId == ActorId && m.FilmId == FilmId);
            if (actorsInFilm == null)
            {
                return NotFound();
            }

            return View(actorsInFilm);
        }

        // POST: ActorsInFilms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ActorId, int FilmId)
        {
            if (_context.ActorsInFilms == null)
            {
                return Problem("Entity set 'LabaFilmsDBContext.ActorsInFilms'  is null.");
            }
            var actorsInFilm = await _context.ActorsInFilms.FirstOrDefaultAsync(x => (x.ActorId == ActorId && x.FilmId == FilmId));
            if (actorsInFilm != null)
            {
                _context.ActorsInFilms.Remove(actorsInFilm);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorsInFilmExists(int ActorId, int FilmId)
        {
          return (_context.ActorsInFilms?.Any(e => e.ActorId == ActorId && e.FilmId == FilmId)).GetValueOrDefault();
        }
        public bool NotExists(int ActorId, int FilmId)
        {
            var tmp = _context.ActorsInFilms.FirstOrDefault(e => e.ActorId == ActorId && e.FilmId == FilmId);
            if (tmp == null) return true;
            return false;
        }
    }
}
