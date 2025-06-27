using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjektDyplomowy.Data;
using ProjektDyplomowy.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjektDyplomowy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var ratings = _context.Rating
            .Include(r => r.User) // £adowanie danych u¿ytkownika
            .ToList();
            return View(ratings);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRating(Rating rating)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Message"] = "You have to be authenticated to add rating.";

                return Redirect("Index#formularz-opinii");
            }

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return Unauthorized();
                }

                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    return Unauthorized();
                }

                rating.UserId = user.Id;
                rating.User = user;

                _context.Rating.Add(rating);
                _context.SaveChanges();


                return Redirect("Index#formularz-opinii");
            }

            return Redirect("Index#formularz-opinii");
        }
        public IActionResult HowItWorks()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
