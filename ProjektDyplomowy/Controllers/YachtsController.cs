using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet]
        public IActionResult GetUnavailableDates(int yachtId)
        {
            var yacht = _context.Yacht.Include(c => c.Rentals).FirstOrDefault(c => c.Id == yachtId);

            if (yacht == null)
            {
                return NotFound();
            }

            // Lista zablokowanych dat
            var unavailableDates = yacht.Rentals.Select(r => new
            {
                from = r.RentalStart.ToString("yyyy-MM-dd"), // Data rozpoczęcia
                to = r.RentalEnd.ToString("yyyy-MM-dd")     // Data zakończenia
            }).ToList();

            return Json(unavailableDates);
        }
        // GET: Yacht/Details
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.YachtId = id;
            var yacht = await _context.Yacht.FirstOrDefaultAsync(c => c.Id == id);

            return View(yacht);
        }

        public async Task<IActionResult> YachtsMain(decimal? minPrice, decimal? maxPrice, List<string> brands)
        {
            var yachtsQuery = _context.Yacht.AsQueryable();

            // Filtrowanie po marce
            if (brands != null && brands.Any())
            {
               yachtsQuery = yachtsQuery.Where(c => brands.Contains(c.Brand));
            }

            // Filtrowanie po cenie
            if (minPrice.HasValue)
            {
                yachtsQuery = yachtsQuery.Where(c => c.DailyRate >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                yachtsQuery = yachtsQuery.Where(c => c.DailyRate <= maxPrice.Value);
            }

            var cars = await yachtsQuery.ToListAsync();
            // Pobierz wszystkie unikalne marki do ViewBag
            var allBrands = await _context.Yacht.Select(c => c.Brand).Distinct().ToListAsync();
            ViewBag.Brands = allBrands;
            return View(cars);
        }

    }
}
