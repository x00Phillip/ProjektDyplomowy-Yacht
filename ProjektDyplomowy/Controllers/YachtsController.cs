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

    }
}
