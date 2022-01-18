using JCAirbnb.Areas.Client.Models;
using JCAirbnb.Data;
using JCAirbnb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("{area}/{action}/{id?}")]
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
        public async Task<IActionResult> Index()
        {
            var client = await _userManager.GetUserAsync(User);

            var reservations = _context.Reservations
                .Include(r => r.User)
                .Include(r => r.ReservationState)
                .Include(r => r.Property).ThenInclude(p => p.Manager)
                .Include(r => r.Property).ThenInclude(p => p.PropertyType)
                .Where(r => r.User.Id == client.Id);

            return View(await reservations.OrderBy(r => r.CheckIn).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> CheckOut(string id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.ReservationState)
                .Include(r => r.Property).ThenInclude(p => p.Manager)
                .Include(r => r.Property).ThenInclude(p => p.PropertyType)
                .FirstOrDefaultAsync(r => r.Id == id);
            var client = await _userManager.GetUserAsync(User);

            if (reservation == null || reservation.User.Id != client.Id || reservation.ReservationState.Title != "Checked in") return NotFound();

            return View(new CheckOutViewModel()
            {
                Reservation = reservation
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(string id, [Bind("Reservation,Ratings,Comment")] CheckOutViewModel viewModel)
        {
            if (id == null || id != viewModel.Reservation.Id) return NotFound();

            var reservation = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Property)
                .FirstOrDefaultAsync(r => r.Id == id);
            var client = await _userManager.GetUserAsync(User);

            if (reservation == null || reservation.User.Id != client.Id) return NotFound();

            var property = await _context.Properties
                .Include(p => p.Ratings)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == reservation.Property.Id);

            property.Ratings += viewModel.Ratings;
            property.Ratings /= property.Reviews.Count;

            property.Reviews.Add(new Review()
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.UtcNow,
                Text = viewModel.Comment,
                User = client
            });

            reservation.ReservationState = await _context.ReservationStates.FirstOrDefaultAsync(rs => rs.Title == "Checked out");

            //_context.Update(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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

                var user = await _userManager.GetUserAsync(User);

                var reservations = _context.Reservations.Include(r => r.Property).Where(r => r.Property.Id == id || r.User.Id == user.Id);

                if (reservations.Any(r => viewModel.CheckIn >= r.CheckIn && viewModel.CheckIn <= r.CheckOut
                                    || viewModel.CheckOut >= r.CheckIn && viewModel.CheckOut <= r.CheckOut))
                    return BadRequest("There is a reservation for that date already or you already have a reservation on that date!");

                _context.Reservations.Add(new Reservation()
                {
                    Id = Guid.NewGuid().ToString(),
                    CheckIn = viewModel.CheckIn,
                    CheckOut = viewModel.CheckOut,
                    Property = viewModel.Property,
                    User = user,
                    ReservationState = await _context.ReservationStates.FirstOrDefaultAsync(rs => rs.Title == "Pending")
                });

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Profile(string id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return NotFound();

            id ??= user.Id;

            var client = await _context.Clients
                .Include(c => c.User)
                .Include(c => c.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(c => c.User.Id == id);

            return View(client);
        }
    }
}
