using JCAirbnb.Data;
using JCAirbnb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View(_context.Properties);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Property(string id)
        {
            if (id == null) return NotFound();

            var property = await _context.Properties
                .Include(p => p.Manager)
                .Include(p => p.Divisions)
                .Include(p => p.PropertyType)
                .Include(p => p.Ratings)
                .Include(p => p.Photos)
                .Include(p => p.Commodities)
                .ThenInclude(c => c.Commodity)
                .Include(p => p.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null) return NotFound();

            return View(property);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
