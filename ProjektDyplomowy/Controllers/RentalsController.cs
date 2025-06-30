using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektDyplomowy.Data;
using ProjektDyplomowy.Models;

namespace ProjektDyplomowy.Controllers
{
    public class RentalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RentalsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> OrderConfirmation(string selectedDates, int yachtId)
        {
            // selectedDates zawiera wybrane daty w formacie "YYYY-MM-DD to YYYY-MM-DD"
            var dates = selectedDates.Split(" to "); // Rozdzielenie zakresu dat
            DateTime startDate = DateTime.Parse(dates[0]);
            DateTime endDate = DateTime.Parse(dates[1]);

            var yacht = await _context.Yacht.FindAsync(yachtId);
            if (yacht == null)
            {
                return NotFound("Yacht not found.");
            }

            int rentalDays = (endDate - startDate).Days;
            decimal totalPrice = rentalDays * yacht.DailyRate;

            var viewModel = new ConfirmOrderViewModel
            {
                StartDate = startDate,
                EndDate = endDate,
                YachtId = yacht.Id,
                YachtBrand = yacht.Brand,
                YachtModel = yacht.Model,
                PricePerDay = yacht.DailyRate,
                TotalPrice = totalPrice
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> FinalizeOrder(ConfirmOrderViewModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var rental = new Rental
            {
                YachtId = model.YachtId,
                Yacht = await _context.Yacht.FindAsync(model.YachtId),
                RentalStart = model.StartDate,
                RentalEnd = model.EndDate,
                ClientId = user.Id,
                Client = user,
            };

            rental.Price = rental.CalculatePrice();

            _context.Rental.Add(rental);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation", new { rentalId = rental.Id });
        }
        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
