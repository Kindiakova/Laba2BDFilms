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
    public class CustomersController : Controller
    {
        private readonly LabaFilmsDBContext _context;
        private readonly static RequestsController request = new RequestsController();

        public CustomersController(LabaFilmsDBContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(int? Id, int? val, int? Rnumber)
        {
            ViewBag.contextActors = _context.Actors.ToList();
            ViewBag.LastRequest = null;
            ViewBag.LastSearch = null;
            ViewBag.LastValue = null;
            ViewBag.ErrorMessage = null;

            if (Rnumber == null) return View(await _context.Customers.ToListAsync());

            if (Rnumber == 2)
            {
                ViewBag.LastRequest = "2";
                ViewBag.LastSearch = Id;
                if (val == 0) ViewBag.LastValue = "";
                if (val == 1) ViewBag.LastValue = "у головній ролі";
                if (val == 2) ViewBag.LastValue = "у другорядній ролі";
                return View(request.Request2((int)Id, (int)val));
            }
            if (Rnumber == 6)
            {
                ViewBag.LastRequest = "6";
                ViewBag.LastValue = val;
                if (val == null || val < 0)
                {
                    ViewBag.ErrorMassage = "Значення суми має бути >= 0";
                    ViewBag.LastValue = null;
                    return View(await _context.Customers.ToListAsync());
                }
                return View(request.Request6((int)val));
            }
            ViewBag.ErrorMassage = "Unknown request";

            return View(await _context.Customers.ToListAsync());
        }

 
        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,LastActiveDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,LastActiveDate")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'LabaFilmsDBContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public bool NameNotExists(string Email)
        {
            var find = _context.Customers.FirstOrDefault(x => x.Email == Email);
            if (find == null) return true;
            else return false;
        }
    }
}
