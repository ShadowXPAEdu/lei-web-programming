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
    public class ManageCommoditiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManageCommoditiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ManageCommodities
        [Route("/{area}/{controller}")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Commodities.ToListAsync());
        }

        // GET: Admin/ManageCommodities/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var commodity = await _context.Commodities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commodity == null) return NotFound();

            return View(commodity);
        }

        // GET: Admin/ManageCommodities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ManageCommodities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description")] Commodity commodity)
        {
            if (ModelState.IsValid)
            {
                commodity.Id = Guid.NewGuid().ToString();
                _context.Add(commodity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(commodity);
        }

        // GET: Admin/ManageCommodities/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var commodity = await _context.Commodities.FindAsync(id);
            if (commodity == null) return NotFound();
            return View(commodity);
        }

        // POST: Admin/ManageCommodities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Description")] Commodity commodity)
        {
            if (id != commodity.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commodity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommodityExists(commodity.Id))
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
            return View(commodity);
        }

        // GET: Admin/ManageCommodities/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commodity = await _context.Commodities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commodity == null)
            {
                return NotFound();
            }

            return View(commodity);
        }

        // POST: Admin/ManageCommodities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var commodity = await _context.Commodities.FindAsync(id);
            _context.Commodities.Remove(commodity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommodityExists(string id)
        {
            return _context.Commodities.Any(e => e.Id == id);
        }
    }
}
