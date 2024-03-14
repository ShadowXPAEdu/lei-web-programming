using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JCAirbnb.Data;
using JCAirbnb.Models;
using Microsoft.AspNetCore.Authorization;
using System;

namespace JCAirbnb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/{controller}/{action}/{id?}")]
    [Authorize(Roles = "Administrator")]
    public class ManagePropertyTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagePropertyTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ManagePropertyTypes
        [Route("/{area}/{controller}/{id?}")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.PropertyTypes.ToListAsync());
        }

        // GET: Admin/ManagePropertyTypes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyType = await _context.PropertyTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (propertyType == null)
            {
                return NotFound();
            }

            return View(propertyType);
        }

        // GET: Admin/ManagePropertyTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ManagePropertyTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description")] PropertyType propertyType)
        {
            if (ModelState.IsValid)
            {
                propertyType.Id = Guid.NewGuid().ToString();
                _context.Add(propertyType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(propertyType);
        }

        // GET: Admin/ManagePropertyTypes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyType = await _context.PropertyTypes.FindAsync(id);
            if (propertyType == null)
            {
                return NotFound();
            }
            return View(propertyType);
        }

        // POST: Admin/ManagePropertyTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Description")] PropertyType propertyType)
        {
            if (id != propertyType.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(propertyType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyTypeExists(propertyType.Id))
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
            return View(propertyType);
        }

        // GET: Admin/ManagePropertyTypes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyType = await _context.PropertyTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (propertyType == null)
            {
                return NotFound();
            }

            return View(propertyType);
        }

        // POST: Admin/ManagePropertyTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var propertyType = await _context.PropertyTypes.FindAsync(id);
            _context.PropertyTypes.Remove(propertyType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyTypeExists(string id)
        {
            return _context.PropertyTypes.Any(e => e.Id == id);
        }
    }
}
