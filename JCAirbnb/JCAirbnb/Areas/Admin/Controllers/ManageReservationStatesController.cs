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

namespace JCAirbnb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/{controller}/{action}/{id?}")]
    [Authorize(Roles = "Administrator")]
    public class ManageReservationStatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManageReservationStatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ManageReservationStates
        [Route("/{area}/{controller}/{id?}")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReservationStates.ToListAsync());
        }

        // GET: Admin/ManageReservationStates/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationState = await _context.ReservationStates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationState == null)
            {
                return NotFound();
            }

            return View(reservationState);
        }

        // GET: Admin/ManageReservationStates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ManageReservationStates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] ReservationState reservationState)
        {
            if (ModelState.IsValid)
            {
                reservationState.Id = Guid.NewGuid().ToString();
                _context.Add(reservationState);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservationState);
        }

        // GET: Admin/ManageReservationStates/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationState = await _context.ReservationStates.FindAsync(id);
            if (reservationState == null)
            {
                return NotFound();
            }
            return View(reservationState);
        }

        // POST: Admin/ManageReservationStates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title")] ReservationState reservationState)
        {
            if (id != reservationState.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservationState);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationStateExists(reservationState.Id))
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
            return View(reservationState);
        }

        // GET: Admin/ManageReservationStates/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationState = await _context.ReservationStates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationState == null)
            {
                return NotFound();
            }

            return View(reservationState);
        }

        // POST: Admin/ManageReservationStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var reservationState = await _context.ReservationStates.FindAsync(id);
            _context.ReservationStates.Remove(reservationState);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationStateExists(string id)
        {
            return _context.ReservationStates.Any(e => e.Id == id);
        }
    }
}
