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
    public class SkipperLicensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SkipperLicensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SkipperLicenses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SkipperLicense.Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SkipperLicenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skipperLicense = await _context.SkipperLicense
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (skipperLicense == null)
            {
                return NotFound();
            }

            return View(skipperLicense);
        }

        // GET: SkipperLicenses/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: SkipperLicenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,LicenseNumber,RadioOperatorLicense,ValidUntil")] SkipperLicense skipperLicense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(skipperLicense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", skipperLicense.UserId);
            return View(skipperLicense);
        }

        // GET: SkipperLicenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skipperLicense = await _context.SkipperLicense.FindAsync(id);
            if (skipperLicense == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", skipperLicense.UserId);
            return View(skipperLicense);
        }

        // POST: SkipperLicenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,LicenseNumber,RadioOperatorLicense,ValidUntil")] SkipperLicense skipperLicense)
        {
            if (id != skipperLicense.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(skipperLicense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkipperLicenseExists(skipperLicense.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", skipperLicense.UserId);
            return View(skipperLicense);
        }

        // GET: SkipperLicenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skipperLicense = await _context.SkipperLicense
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (skipperLicense == null)
            {
                return NotFound();
            }

            return View(skipperLicense);
        }

        // POST: SkipperLicenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var skipperLicense = await _context.SkipperLicense.FindAsync(id);
            if (skipperLicense != null)
            {
                _context.SkipperLicense.Remove(skipperLicense);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SkipperLicenseExists(int id)
        {
            return _context.SkipperLicense.Any(e => e.Id == id);
        }
    }
}
