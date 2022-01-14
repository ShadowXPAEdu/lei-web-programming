using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JCAirbnb.Data;
using JCAirbnb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using JCAirbnb.Areas.Manager.Models;

namespace JCAirbnb.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("{area}/{controller}/{action}/{id?}")]
    [Authorize(Roles = "Manager")]
    public class ManageReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ManageReservationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("/{area}/{controller}/{id?}")]
        // GET: Manager/ManageReservations
        public async Task<IActionResult> Index()
        {
            var manager = await _userManager.GetUserAsync(User);

            var reservationStates = _context.ReservationStates.Where(rs => rs.Title == "Pending" || rs.Title == "Finalized");

            var reservations = _context.Reservations
                .Include(r => r.Property)
                    .ThenInclude(p => p.Manager)
                .Include(r => r.ReservationState)
                .Include(r => r.User)
                .Where(r => r.Property.Manager.Id == manager.Id && reservationStates.Contains(r.ReservationState));

            return View(await reservations.ToListAsync());
        }

        // GET: Manager/ManageReservations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _userManager.GetUserAsync(User);
            var reservation = await _context.Reservations
                .Include(r => r.Property).ThenInclude(p => p.Manager)
                .Include(r => r.Property).ThenInclude(p => p.PropertyType)
                .Include(r => r.ReservationState)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (reservation == null || reservation.Property.Manager.Id != manager.Id)
            {
                return NotFound();
            }
            return View(new ManageReservationsEditViewModel()
            {
                Reservation = reservation,
                CheckLists = await _context.CheckLists.Include(cl => cl.Company).Where(cl => cl.Company.Id == manager.Id).ToListAsync()
            });
        }

        // POST: Manager/ManageReservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Reservation,CheckListId")] ManageReservationsEditViewModel viewModel)
        {
            if (id != viewModel.Reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var reservation = await _context.Reservations.FindAsync(id);
                    reservation.ReservationCheckList = await _context.CheckLists.FindAsync(viewModel.CheckListId);
                    reservation.ReservationState = await _context.ReservationStates.FirstOrDefaultAsync(rs => rs.Title == "Reserved");
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(viewModel.Reservation.Id))
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
            return View(viewModel);
        }

        // GET: Manager/ManageReservations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _userManager.GetUserAsync(User);
            var reservation = await _context.Reservations
                .Include(r => r.Property)
                    .ThenInclude(p => p.Manager)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null || reservation.Property.Manager.Id != manager.Id)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Manager/ManageReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(string id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
