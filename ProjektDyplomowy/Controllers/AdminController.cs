using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjektDyplomowy.Data;
using ProjektDyplomowy.Models;
using Microsoft.CodeAnalysis;

namespace ProjektDyplomowy.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AdminController> _logger;
        public AdminController(
         UserManager<IdentityUser> userManager,
         RoleManager<IdentityRole> roleManager,
         ApplicationDbContext context, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }
        // Ta metoda ustawia ViewData["IsAdmin"] dla wszystkich akcji w tym kontrolerze
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Sprawdza, czy użytkownik ma rolę "Administrator"
            ViewData["IsAdmin"] = User.IsInRole("Admin");
            base.OnActionExecuting(context);
        }
        public IActionResult Index()
        {
            return View();
        }
        // Wyświetlenie listy jachtów
        public async Task<IActionResult> Yachts()
        {
            var yachts = await _context.Yacht.ToListAsync();
            return PartialView("_YachtList", yachts);
        }
        // GET
        public async Task<IActionResult> CreateYacht()
        {
            var partners = await _userManager.GetUsersInRoleAsync("Partner");
            ViewBag.Partners = new SelectList(partners, "Id", "Email");
            ViewBag.Locations = new SelectList(_context.Yacht_Location, "Id", "Name");

            return PartialView("_YachtCreate", new Yacht());
        }
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateYacht([Bind("Id,Brand,Model,Year,LengthInMeters,MaxPersons,DailyRate,NumberOfCabins,NumberOfBathrooms,OwnerId,Yacht_LocationId,Type,HasKitchen,HasAirConditioning,HasWiFi,Image")] Yacht yacht)
        {
            Yacht y = new()
            {
                Brand = yacht.Brand,
                Model = yacht.Model,
                Year = yacht.Year,
                LengthInMeters = yacht.LengthInMeters,
                MaxPersons = yacht.MaxPersons,
                DailyRate = yacht.DailyRate,
                NumberOfBathrooms = yacht.NumberOfBathrooms,
                NumberOfCabins = yacht.NumberOfCabins,
                Type = yacht.Type,
                HasAirConditioning = yacht.HasAirConditioning,
                HasKitchen = yacht.HasKitchen,
                HasWiFi = yacht.HasWiFi,
                OwnerId = yacht.OwnerId,
                Yacht_LocationId = yacht.Yacht_LocationId
            };
            _context.Add(y);
            await _context.SaveChangesAsync();

            var partners = await _userManager.GetUsersInRoleAsync("Partner");
            ViewBag.Partners = new SelectList(partners, "Id", "Email", yacht.OwnerId);
            ViewBag.Locations = new SelectList(_context.Yacht_Location, "Id", "Name", yacht.Yacht_LocationId);
            return PartialView("_YachtCreate", yacht);
        }
        // Edycja jachtu (GET)
        public async Task<IActionResult> EditYacht(int id)
        {
            var yacht = await _context.Yacht.FindAsync(id);
            if (yacht == null)
            {
                return NotFound();
            }
            var partners = await _userManager.GetUsersInRoleAsync("Partner");
            ViewBag.Partners = new SelectList(partners, "Id", "Email", yacht.OwnerId);
            ViewBag.Locations = new SelectList(_context.Yacht_Location, "Id", "Name", yacht.Yacht_LocationId);
            return PartialView("_YachtEdit", yacht);
        }
        // Edycja jachtu (POST)
        [HttpPost]
        public async Task<IActionResult> EditYacht(Yacht yacht)
        {
            if (ModelState.IsValid)
            {
                _context.Yacht.Update(yacht);
                await _context.SaveChangesAsync();
                var yachts = await _context.Yacht
                .Include(y => y.Owner)
                .Include(y => y.Yacht_Location)
                .ToListAsync();
                return PartialView("_YachtList", yachts);
            }
            var partners = await _userManager.GetUsersInRoleAsync("Partner");
            ViewBag.Partners = new SelectList(partners, "Id", "Email", yacht.OwnerId);
            ViewBag.Locations = new SelectList(_context.Yacht_Location, "Id", "Name", yacht.Yacht_LocationId);
            return PartialView("_YachtEdit", yacht);
        }

        // Usunięcie jachtu
        public async Task<IActionResult> DeleteYacht(int id)
        {
            var yacht = await _context.Yacht.FindAsync(id);
            if (yacht == null)
            {
                return NotFound();
            }
            _context.Yacht.Remove(yacht);
            await _context.SaveChangesAsync();

            var yachts = await _context.Yacht.ToListAsync();
            return PartialView("_YachtList", yachts);
        }
        // wyświetlenie użytkowników
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return PartialView("_UserList", users);
        }
        // Dodanie użytkownika (GET)
        public async Task<IActionResult> CreateUser()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.Roles = roles; // Przekaż role do widoku
            return PartialView("_UserCreate");
        }
        //Dodanie użytkownika (POST)
        [HttpPost]
        public async Task<IActionResult> CreateUser(string email, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                ModelState.AddModelError("", "All fields are required.");
                return PartialView("_UserCreate");
            }

            var user = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            // Tworzenie użytkownika
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return PartialView("_UserCreate");
            }

            // Przypisanie roli
            if (!await _roleManager.RoleExistsAsync(role))
            {
                ModelState.AddModelError("", "Role does not exist.");
                return PartialView("_UserCreate");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return PartialView("_UserCreate");
            }

            // Jeśli wszystko się udało, przeładuj listę użytkowników
            var users = await _userManager.Users.ToListAsync();
            return PartialView("_UserList", users);
        }
        // Edycja użytkownika (GET)
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.Roles = roles;

            return PartialView("_UserEdit", user);
        }
        //Edycja użytkownika (POST)
        [HttpPost]
        public async Task<IActionResult> EditUser(string id, IdentityUser updatedUser, string role)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // Aktualizuj dane użytkownika
                user.UserName = updatedUser.UserName;
                user.Email = updatedUser.Email;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update user.");
                    var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                    ViewBag.Roles = roles;
                    return PartialView("_UserEdit", updatedUser);
                }

                // Aktualizacja roli użytkownika
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Count > 0)
                {
                    await _userManager.RemoveFromRolesAsync(user, userRoles);
                }

                if (!string.IsNullOrEmpty(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }

                // Pobierz zaktualizowaną listę użytkowników
                var users = await _userManager.Users.ToListAsync();
                return PartialView("_UserList", users);
            }

            var availableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.Roles = availableRoles;

            return PartialView("_UserEdit", updatedUser);
        }
        //Usunięcie użytkownika (GET)
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            var users = await _context.Users.ToListAsync();
            return PartialView("_UserList", users);

        }
        // Wyświetlenie listy wypożyczeń
        public async Task<IActionResult> Rentals()
        {
            var rentals = await _context.Rental
                .Include(r => r.Yacht)
                .Include(r => r.Client)
                .ToListAsync();
            return PartialView("_RentalList", rentals);
        }
        // Dodanie wypożyczenia (GET)
        public IActionResult CreateRental()
        {
            ViewBag.ClientId = new SelectList(_userManager.Users.ToList(), "Id", "UserName");
            ViewBag.CarId = new SelectList(_context.Yacht.ToList(), "Id", "Brand");
            return PartialView("_RentalCreate");
        }
        // Dodanie wypożyczenia (POST)
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,ClientId,CarId,RentalStart,RentalEnd,Price")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                rental.Client = await _userManager.FindByIdAsync(rental.ClientId);
                rental.Yacht = await _context.Yacht.FindAsync(rental.YachtId);
                if (rental.Client == null || rental.Yacht == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Client or Car selection.");
                    return PartialView("_RentalCreate", rental);
                }
                _context.Add(rental);
                var rentals = await _context.Rental.Include(r => r.Client).Include(r => r.Yacht).ToListAsync();
                return PartialView("_RentalList", rentals);
            }
            ViewBag.ClientId = new SelectList(_userManager.Users.ToList(), "Id", "UserName", rental.ClientId);
            ViewBag.CarId = new SelectList(_context.Yacht.ToList(), "Id", "Brand", rental.YachtId);
            return PartialView("_RentalList");
        }
        //Edytowanie wypożyczenia (GET)
        public async Task<IActionResult> EditRental(int id)
        {
            // Znajdź wypożyczenie w bazie danych na podstawie jego Id
            var rental = await _context.Rental
                .Include(r => r.Client)
                .Include(r => r.Yacht)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (rental == null)
            {
                return NotFound(); // Zwróć błąd 404, jeśli wypożyczenie nie istnieje
            }
            // Przygotuj dane do wyświetlenia w formularzu
            ViewBag.ClientId = new SelectList(_userManager.Users, "Id", "UserName", rental.ClientId);
            ViewBag.CarId = new SelectList(_context.Yacht, "Id", "Brand", rental.YachtId);

            return PartialView("_RentalEdit", rental); // Zwróć widok edycji
        }
        // Zapisanie zmian w wypożyczeniu (POST)
        [HttpPost]
        public async Task<IActionResult> EditRental(int id, [Bind("Id,ClientId,CarId,RentalStart,RentalEnd,Price")] Rental rental)
        {
            if (id != rental.Id)
            {
                return BadRequest(); // Zwróć błąd 400, jeśli Id w modelu nie zgadza się z Id w URL
            }
            if (ModelState.IsValid)
            {
                try
                {
                    // Znajdź istniejące wypożyczenie w bazie danych
                    var existingRental = await _context.Rental.FindAsync(id);

                    if (existingRental == null)
                    {
                        return NotFound(); // Zwróć błąd 404, jeśli wypożyczenie nie istnieje
                    }
                    // Aktualizuj właściwości wypożyczenia
                    existingRental.ClientId = rental.ClientId;
                    existingRental.YachtId = rental.YachtId;
                    existingRental.RentalStart = rental.RentalStart;
                    existingRental.RentalEnd = rental.RentalEnd;

                    // Zapisz zmiany w bazie danych
                    _context.Rental.Update(existingRental);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Rentals"); // Przekierowanie do listy wypożyczeń
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(rental.Id))
                    {
                        return NotFound(); // Jeśli wypożyczenie zostało usunięte w międzyczasie
                    }
                    else
                    {
                        throw; // Ponów wyjątek, jeśli wystąpił inny problem
                    }
                }
            }

            // Przygotuj dane do widoku w przypadku błędu
            ViewBag.ClientId = new SelectList(_userManager.Users, "Id", "UserName", rental.ClientId);
            ViewBag.CarId = new SelectList(_context.Yacht, "Id", "Brand", rental.YachtId);

            return PartialView("_RentalEdit", rental); // Zwróć widok edycji z błędami
        }
        private bool RentalExists(int id)
        {
            return _context.Rental.Any(r => r.Id == id);
        }
        // Usunięcie wypożyczenia
        public async Task<IActionResult> DeleteRental(int id)
        {
            var rental = await _context.Rental.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            _context.Rental.Remove(rental);
            await _context.SaveChangesAsync();
            var rentals = await _context.Rental.ToListAsync();
            return PartialView("_RentalList", rentals);
        }
        // Wyświetlenie listy lokalizacji
        public async Task<IActionResult> YachtLocations()
        {
            var locations = await _context.Yacht_Location.ToListAsync();
            return PartialView("_YachtLocationList", locations);
        }
        // Dodanie nowej lokalizacji (GET)
        public IActionResult CreateYachtLocation()
        {
            return PartialView("_YachtLocationCreate", new Yacht_Location());
        }

        // Dodanie nowej lokalizacji (POST)
        [HttpPost]
        public async Task<IActionResult> CreateYachtLocation(Yacht_Location yacht_Location)
        {
            if (ModelState.IsValid)
            {
                _context.Yacht_Location.Add(yacht_Location);
                await _context.SaveChangesAsync();
                var locations = await _context.Yacht_Location.ToListAsync();
                return PartialView("_YachtLocationList", locations);
            }
            return PartialView("_YachtLocationCreate", yacht_Location);
        }

        // Edycja lokalizacji (GET)
        public async Task<IActionResult> EditYachtLocation(int id)
        {
            var location = await _context.Yacht_Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return PartialView("_YachtLocationEdit", location);
        }

        // Edycja lokalizacji (POST)
        [HttpPost]
        public async Task<IActionResult> EditYachtLocation(Yacht_Location yacht_Location)
        {
            if (ModelState.IsValid)
            {
                _context.Yacht_Location.Update(yacht_Location);
                await _context.SaveChangesAsync();
                var locations = await _context.Yacht_Location.ToListAsync();
                return PartialView("_YachtLocationList", locations);
            }
            return PartialView("_YachtLocationEdit", yacht_Location);
        }

        // Usunięcie lokalizacji
        public async Task<IActionResult> DeleteYachtLocation(int id)
        {
            var location = await _context.Yacht_Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            _context.Yacht_Location.Remove(location);
            await _context.SaveChangesAsync();

            var locations = await _context.Yacht_Location.ToListAsync();
            return PartialView("_YachtLocationList", locations);
        }


    }
}
