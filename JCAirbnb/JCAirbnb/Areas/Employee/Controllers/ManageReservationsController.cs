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
using JCAirbnb.Areas.Employee.Models;

namespace JCAirbnb.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Route("{area}/{action}/{id?}")]
    [Authorize(Roles = "Manager,Employee")]
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
        // GET: Employee/ManageReservations
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var reservationStates = _context.ReservationStates.Where(rs => rs.Title == "Reserved" || rs.Title == "CheckedOut");

            var allCompanies = await _context.Companies.Include(c => c.Manager).Include(c => c.Employees).ToListAsync();
            var allUserEmployees = await _context.Employees.Include(e => e.User).Where(e => e.User.Id == user.Id).ToListAsync();

            var companies = new List<Company>();

            foreach (var company in allCompanies)
                foreach (var employee in allUserEmployees)
                    if (company.Employees.Contains(employee))
                        companies.Add(company);

            var reservations = _context.Reservations
                .Include(r => r.Property)
                    .ThenInclude(p => p.Manager)
                .Include(r => r.ReservationState)
                .Include(r => r.User)
                .Where(r => reservationStates.Contains(r.ReservationState)
                         && (r.Property.Manager.Id == user.Id || companies.Select(c => c.Manager).Contains(r.Property.Manager)));

            return View(await reservations.ToListAsync());
        }

        // GET: Employee/ManageReservations/Edit/5
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
            //var reservation = await _context.Reservations.FindAsync(id);
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

        // POST: Employee/ManageReservations/Edit/5
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
                    reservation.CheckList = await _context.CheckLists.FindAsync(viewModel.CheckListId);
                    //if(reservation.ReservationState == "Reserved"){}
                    //if (reservation.ReservationState == "CheckedOut"){}
                    reservation.ReservationState = await _context.ReservationStates.FirstOrDefaultAsync(rs => rs.Title == "Checked in");
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

        // GET: Employee/ManageReservations/Delete/5
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

        // POST: Employee/ManageReservations/Delete/5
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
