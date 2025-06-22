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
    public class YachtsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public YachtsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Yachts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Yacht.Include(y => y.Owner).Include(y => y.Yacht_Location);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Yachts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yacht = await _context.Yacht
                .Include(y => y.Owner)
                .Include(y => y.Yacht_Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yacht == null)
            {
                return NotFound();
            }

            return View(yacht);
        }

        // GET: Yachts/Create
        public IActionResult Create()
        {
            ViewData["OwnerId"] = new SelectList(_context.Set<Owner>(), "UserId", "UserId");
            ViewData["Yacht_LocationId"] = new SelectList(_context.Set<Yacht_Location>(), "Id", "Name");
            return View();
        }

        // POST: Yachts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Brand,Model,Year,LengthInMeters,MaxPersons,DailyRate,NumberOfCabins,NumberOfBathrooms,OwnerId,Yacht_LocationId,Type,HasKitchen,HasAirConditioning,HasWiFi,Image")] Yacht yacht)
        {
            if (ModelState.IsValid)
            {
                _context.Add(yacht);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerId"] = new SelectList(_context.Set<Owner>(), "UserId", "UserId", yacht.OwnerId);
            ViewData["Yacht_LocationId"] = new SelectList(_context.Set<Yacht_Location>(), "Id", "Name", yacht.Yacht_LocationId);
            return View(yacht);
        }

        // GET: Yachts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yacht = await _context.Yacht.FindAsync(id);
            if (yacht == null)
            {
                return NotFound();
            }
            ViewData["OwnerId"] = new SelectList(_context.Set<Owner>(), "UserId", "UserId", yacht.OwnerId);
            ViewData["Yacht_LocationId"] = new SelectList(_context.Set<Yacht_Location>(), "Id", "Name", yacht.Yacht_LocationId);
            return View(yacht);
        }

        // POST: Yachts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,Model,Year,LengthInMeters,MaxPersons,DailyRate,NumberOfCabins,NumberOfBathrooms,OwnerId,Yacht_LocationId,Type,HasKitchen,HasAirConditioning,HasWiFi,Image")] Yacht yacht)
        {
            if (id != yacht.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(yacht);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YachtExists(yacht.Id))
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
            ViewData["OwnerId"] = new SelectList(_context.Set<Owner>(), "UserId", "UserId", yacht.OwnerId);
            ViewData["Yacht_LocationId"] = new SelectList(_context.Set<Yacht_Location>(), "Id", "Name", yacht.Yacht_LocationId);
            return View(yacht);
        }

        // GET: Yachts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yacht = await _context.Yacht
                .Include(y => y.Owner)
                .Include(y => y.Yacht_Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yacht == null)
            {
                return NotFound();
            }

            return View(yacht);
        }

        // POST: Yachts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var yacht = await _context.Yacht.FindAsync(id);
            if (yacht != null)
            {
                _context.Yacht.Remove(yacht);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool YachtExists(int id)
        {
            return _context.Yacht.Any(e => e.Id == id);
        }
    }
}
