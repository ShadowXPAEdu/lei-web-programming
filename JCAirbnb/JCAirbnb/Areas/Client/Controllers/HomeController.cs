using JCAirbnb.Areas.Client.Models;
using JCAirbnb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("{area}/{controller}/{action}/{id?}")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("/{area}/{id?}")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Reserve(string id, string checkIn, string checkOut)
        {
            if (id == null || string.IsNullOrWhiteSpace(checkIn) || string.IsNullOrWhiteSpace(checkOut)) return NotFound();

            DateTime checkInDate = DateTime.Parse(checkIn);
            DateTime checkOutDate = DateTime.Parse(checkOut);

            if (checkInDate >= checkOutDate) return BadRequest("Check-out date can not be before check-in date.");

            var property = await _context.Properties.FindAsync(id);

            if (property == null) return NotFound();

            ReserveViewModel viewModel = new()
            {
                Property = property,
                CheckIn = checkInDate,
                CheckOut = checkOutDate,
                ExpireDate = DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day + 1)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(string id, [Bind("Property,CheckIn,CheckOut,CardNumber,ExpireDate,CVC")] ReserveViewModel viewModel)
        {
            if (id == null) return NotFound();

            var property = await _context.Properties.FindAsync(id);

            if (property == null) return NotFound();

            viewModel.Property = property;

            if (ModelState.IsValid)
            {
                if (viewModel.CheckIn >= viewModel.CheckOut) return BadRequest("Check-out date can not be before check-in date.");

                // TODO: check reservations
                // Check if property has another reservation during that time
                // Check if client has another reservation during that time
                // Add reservation if successful

            }

            return View(viewModel);
        }
    }
}
