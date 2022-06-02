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
    public class ActorsController : Controller
    {
        private readonly LabaFilmsDBContext _context;
        private readonly static RequestsController request = new RequestsController();

        public ActorsController(LabaFilmsDBContext context)
        {
            _context = context;
        }

        // GET: Actors
        public async Task<IActionResult> Index(int? Id, int? persent, int? val, int? Rnumber)
        {
            if (Rnumber == null) ViewBag.LastRequest = "";
            ViewBag.contextActors = _context.Actors.ToList();
            ViewBag.contextFilms = _context.Films.ToList();
            ViewBag.LastRequest = null;
            ViewBag.LastSearch = null;
            ViewBag.LastValue = null;
            ViewBag.ErrorMassage = null;

            if (Rnumber == null) return View(await _context.Actors.ToListAsync());
            if (Rnumber == 1)
            {
                ViewBag.LastRequest = "1";
                ViewBag.LastSearch = Id;
                return View(request.Request1((int)Id));
            }
            if (Rnumber == 3)
            {
                ViewBag.LastRequest = "3";
                if ((int)val == 1) ViewBag.LastValue = "не";
                else ViewBag.LastValue = "";
                ViewBag.LastSearch = persent;

                if (persent == null || persent < 0 || persent > 100)
                {
                    ViewBag.ErrorMassage = "Значення відсотка має бути від 0 до 100";
                    ViewBag.LastSearch = null;
                    return View(await _context.Actors.ToListAsync());
                }               
                return View(request.Request3((int)persent, (int)val));
            }
            if (Rnumber == 7)
            {
                ViewBag.LastRequest = "7";
                if ((int)val == 0) ViewBag.LastValue = "актори та акторки";
                if ((int)val == 1) ViewBag.LastValue = "актори";
                if ((int)val == 2) ViewBag.LastValue = "акторки";

                ViewBag.LastSearch = Id;
                return View(request.Request7((int)Id, (int)val));
            }
            ViewBag.ErrorMassage = "Unknown request";
            
            return NotFound();
          
        }

        // GET: Actors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Gender,BirthDay")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Actors == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Gender,BirthDay")] Actor actor)
        {
            if (id != actor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.Id))
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
            return View(actor);
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Actors == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Actors == null)
            {
                return Problem("Entity set 'LabaFilmsDBContext.Actors'  is null.");
            }
            var actor = await _context.Actors.FindAsync(id);
            if (actor != null)
            {
                _context.Actors.Remove(actor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
          return (_context.Actors?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public bool NameNotExists(string FirstName, string LastName)
        {
            var find = _context.Actors.FirstOrDefault(x => ((x.FirstName == FirstName) && (x.LastName == LastName)));
            if (find == null) return true;
            else return false;
        }
    }
}
