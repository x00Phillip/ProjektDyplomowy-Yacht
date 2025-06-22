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
    public class SupportMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupportMessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SupportMessages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SupportMessage.Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SupportMessages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportMessage = await _context.SupportMessage
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supportMessage == null)
            {
                return NotFound();
            }

            return View(supportMessage);
        }

        // GET: SupportMessages/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: SupportMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Message,IsFromSupport,Timestamp")] SupportMessage supportMessage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supportMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", supportMessage.UserId);
            return View(supportMessage);
        }

        // GET: SupportMessages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportMessage = await _context.SupportMessage.FindAsync(id);
            if (supportMessage == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", supportMessage.UserId);
            return View(supportMessage);
        }

        // POST: SupportMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Message,IsFromSupport,Timestamp")] SupportMessage supportMessage)
        {
            if (id != supportMessage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supportMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportMessageExists(supportMessage.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", supportMessage.UserId);
            return View(supportMessage);
        }

        // GET: SupportMessages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportMessage = await _context.SupportMessage
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supportMessage == null)
            {
                return NotFound();
            }

            return View(supportMessage);
        }

        // POST: SupportMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supportMessage = await _context.SupportMessage.FindAsync(id);
            if (supportMessage != null)
            {
                _context.SupportMessage.Remove(supportMessage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupportMessageExists(int id)
        {
            return _context.SupportMessage.Any(e => e.Id == id);
        }
    }
}
