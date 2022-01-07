using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JCAirbnb.Data;
using JCAirbnb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace JCAirbnb.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("{area}/{controller}/{action}/{id?}")]
    [Authorize(Roles = "Manager")]
    public class ManagePropertiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ManagePropertiesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Manager/ManageProperties
        [Route("/{area}/{controller}/{id?}")]
        public async Task<IActionResult> Index()
        {
            var aux = _context.Properties.Include(d => d.Divisions).Include(c => c.Commodities);
            return View(await aux.ToListAsync());
        }

        // GET: Manager/ManageProperties/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Properties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // GET: Manager/ManageProperties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manager/ManageProperties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Location,BasePrice,Price")] Property @property, 
            [FromForm(Name = "guest")] int guest, [FromForm(Name = "bedroom")] int bedroom, [FromForm(Name = "bed")] int bed, 
            [FromForm(Name = "bath")] int bath, [FromForm(Name = "privateBath")] int privateBath)
        {
            if (ModelState.IsValid)
            {
                property.Id = Guid.NewGuid().ToString();
                property.Manager = (await _userManager.GetUserAsync(User)); //falta por o manager associado

                property.Divisions = new Divisions();
                property.Divisions.Id = property.Id;
                property.Divisions.Guest = guest;
                property.Divisions.Bedroom = bedroom;
                property.Divisions.Bed = bed;
                property.Divisions.Bath = bath;
                property.Divisions.PrivateBath = privateBath;

                //property.Commodities[0]. = new Commodity();

                _context.Add(@property);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@property);
        }

        // GET: Manager/ManageProperties/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Properties.FindAsync(id);
            if (@property == null)
            {
                return NotFound();
            }
            return View(@property);
        }

        // POST: Manager/ManageProperties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Description,Location,BasePrice,Price")] Property @property)
        {
            if (id != @property.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@property);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(@property.Id))
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
            return View(@property);
        }

        // GET: Manager/ManageProperties/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Properties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // POST: Manager/ManageProperties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var @property = await _context.Properties.FindAsync(id);
            _context.Properties.Remove(@property);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(string id)
        {
            return _context.Properties.Any(e => e.Id == id);
        }
    }
}
