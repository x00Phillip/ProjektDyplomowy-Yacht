using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektDyplomowy.Data;
using ProjektDyplomowy.Models;

namespace ProjektDyplomowy.Controllers
{
    public class Yacht_LocationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Yacht_LocationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Yacht_Location
        public async Task<IActionResult> Index()
        {
            return View(await _context.Yacht_Location.ToListAsync());
        }

        // GET: Yacht_Location/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yacht_Location = await _context.Yacht_Location
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yacht_Location == null)
            {
                return NotFound();
            }

            return View(yacht_Location);
        }

        // GET: Yacht_Location/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Yacht_Location/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,MapUrl")] Yacht_Location yacht_Location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(yacht_Location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(yacht_Location);
        }

        // GET: Yacht_Location/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yacht_Location = await _context.Yacht_Location.FindAsync(id);
            if (yacht_Location == null)
            {
                return NotFound();
            }
            return View(yacht_Location);
        }

        // POST: Yacht_Location/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,MapUrl")] Yacht_Location yacht_Location)
        {
            if (id != yacht_Location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(yacht_Location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Yacht_LocationExists(yacht_Location.Id))
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
            return View(yacht_Location);
        }

        // GET: Yacht_Location/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yacht_Location = await _context.Yacht_Location
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yacht_Location == null)
            {
                return NotFound();
            }

            return View(yacht_Location);
        }

        // POST: Yacht_Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var yacht_Location = await _context.Yacht_Location.FindAsync(id);
            if (yacht_Location != null)
            {
                _context.Yacht_Location.Remove(yacht_Location);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Yacht_LocationExists(int id)
        {
            return _context.Yacht_Location.Any(e => e.Id == id);
        }
    }
}
