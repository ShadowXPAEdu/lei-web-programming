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

            var reservationStates = _context.ReservationStates.Where(rs => rs.Title == "Reserved" || rs.Title == "Checked out");

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

        // GET: Employee/ManageReservations/Reserved/5
        public async Task<IActionResult> Reserved(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _userManager.GetUserAsync(User);
            var reservation = await _context.Reservations
                .Include(r => r.ReservationCheckList)
                //.Include(r => r.DeliveryCheckList)
                .Include(r => r.Property).ThenInclude(p => p.Manager)
                .Include(r => r.Property).ThenInclude(p => p.PropertyType)
                .Include(r => r.ReservationState)
                .FirstOrDefaultAsync(r => r.Id == id);
            //var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null || reservation.Property.Manager.Id != manager.Id)
            {
                return NotFound();
            }
            //return View(reservation);
            return View(new ManageReservationsModel()
            {
                Reservation = reservation,
                CheckListItems = await _context.CheckListItems.Include(cli => cli.CheckList)
                                        .Where(cli => cli.CheckList.Id == reservation.ReservationCheckList.Id)
                                        .OrderBy(r => r.Description).ToListAsync()
            });
        }

        // POST: Employee/ManageReservations/Reserved/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserved(string id,
            [Bind("Reservation")] ManageReservationsModel viewModel, string[] values)
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
                    var checkListItems = await _context.CheckListItems.Include(cli => cli.CheckList)
                                         .Where(cli => cli.CheckList.Id == reservation.ReservationCheckList.Id)
                                         .OrderBy(r => r.Description).ToListAsync();
                    
                    for(int i =0; i < checkListItems.Count(); i++)
                    {
                        if(values[i] == "On")
                        {
                            checkListItems[i].Verified = true;
                        }
                        else
                        {
                            checkListItems[i].Verified = false;
                        }
                    }

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

        // GET: Employee/ManageReservations/CheckedOut/5
        public async Task<IActionResult> CheckedOut(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _userManager.GetUserAsync(User);
            var reservation = await _context.Reservations
                 //.Include(r => r.ReservationCheckList)
                .Include(r => r.DeliveryCheckList)
                .Include(r => r.Report).ThenInclude(rr => rr.Photos)
                .Include(r => r.Property).ThenInclude(p => p.Manager)
                .Include(r => r.Property).ThenInclude(p => p.PropertyType)
                .Include(r => r.ReservationState)
                .FirstOrDefaultAsync(r => r.Id == id);
            //var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null || reservation.Property.Manager.Id != manager.Id)
            {
                return NotFound();
            }
            return View(new ManageReservationsModel()
            {
                Reservation = reservation,
                CheckListItems = await _context.CheckListItems.Include(cli => cli.CheckList)
                                        .Where(cli => cli.CheckList.Id == reservation.DeliveryCheckList.Id)
                                        .OrderBy(r => r.Description).ToListAsync()
            });
        }

        // POST: Employee/ManageReservations/CheckedOut/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckedOut(string id,
            [Bind("Reservation")] ManageReservationsModel viewModel, string[] values)
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
                    var checkListItems = await _context.CheckListItems.Include(cli => cli.CheckList)
                                         .Where(cli => cli.CheckList.Id == reservation.ReservationCheckList.Id)
                                         .OrderBy(r => r.Description).ToListAsync();

                    for (int i = 0; i < checkListItems.Count(); i++)
                    {
                        if (values[i] == "On")
                        {
                            checkListItems[i].Verified = true;
                        }
                        else
                        {
                            checkListItems[i].Verified = false;
                        }
                    }

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
