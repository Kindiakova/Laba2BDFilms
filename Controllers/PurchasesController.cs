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
    public class PurchasesController : Controller
    {
        private readonly LabaFilmsDBContext _context;

        public PurchasesController(LabaFilmsDBContext context)
        {
            _context = context;
        }

        // GET: Purchases
        public async Task<IActionResult> Index()
        {
            var labaFilmsDBContext = _context.Purchases.Include(p => p.Customer).Include(p => p.Film);
            return View(await labaFilmsDBContext.ToListAsync());
        }


        // GET: Purchases/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email");
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Title");
            return View();
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FilmId,CustomerId,Cost,PaymentDay")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", purchase.CustomerId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Title", purchase.FilmId);
            return View(purchase);
        }

        // GET: Purchases/Edit/5
        public async Task<IActionResult> Edit(int? CustomerId, int? FilmId)
        {
            if (CustomerId == null ||FilmId == null|| _context.Purchases == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases.FirstOrDefaultAsync(x =>x.CustomerId == CustomerId & x.FilmId == FilmId);
            if (purchase == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", purchase.CustomerId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Title", purchase.FilmId);
            purchase.Film = await _context.Films.FirstAsync(x => x.Id == purchase.FilmId);
            purchase.Customer = await _context.Customers.FirstAsync(x => x.Id == purchase.CustomerId);
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int CustomerId, int FilmId, [Bind("FilmId,CustomerId,Cost,PaymentDay")] Purchase purchase)
        {
            if (CustomerId != purchase.CustomerId || FilmId != purchase.FilmId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseExists(purchase.FilmId))
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
            purchase.Film = await _context.Films.FirstAsync(x => x.Id == purchase.FilmId);
            purchase.Customer = await _context.Customers.FirstAsync(x => x.Id == purchase.CustomerId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", purchase.CustomerId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Title", purchase.FilmId);
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public async Task<IActionResult> Delete(int? CustomerId, int? FilmId)
        {
            if (CustomerId == null || FilmId == null || _context.Purchases == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
                .Include(p => p.Customer)
                .Include(p => p.Film)
                .FirstOrDefaultAsync(m => m.FilmId == FilmId && m.CustomerId == CustomerId);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CustomerId, int FilmId)
        {
            if (_context.Purchases == null)
            {
                return Problem("Entity set 'LabaFilmsDBContext.Purchases'  is null.");
            }
            var purchase = await _context.Purchases.FirstOrDefaultAsync(x => x.CustomerId == CustomerId & x.FilmId == FilmId);
            if (purchase != null)
            {
                _context.Purchases.Remove(purchase);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseExists(int id)
        {
          return (_context.Purchases?.Any(e => e.FilmId == id)).GetValueOrDefault();
        }
    }
}
