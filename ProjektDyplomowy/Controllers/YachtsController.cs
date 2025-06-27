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
      
        // GET: Yachts/Create
        public IActionResult Create()
        {
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["Yacht_LocationId"] = new SelectList(_context.Yacht_Location, "Id", "Name");
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
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", yacht.OwnerId);
            ViewData["Yacht_LocationId"] = new SelectList(_context.Yacht_Location, "Id", "Name", yacht.Yacht_LocationId);
            return View(yacht);
        }
   
    }
}
