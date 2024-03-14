using JCAirbnb.Areas.Admin.Models;
using JCAirbnb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/{controller}/{action}/{id?}")]
    [Authorize(Roles = "Administrator")]
    public class ManageOwnerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ManageOwnerController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("/{area}/{controller}/{id?}")]
        public async Task<IActionResult> Index()
        {
            var managerId = (await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Manager")).Id;
            var managers = await _context.UserRoles.Where(ur => ur.RoleId == managerId).Select(ur => ur.UserId).ToListAsync();
            return View(_context.Users.Where(u => managers.Contains(u.Id)));
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var manager = await _context.Users.FindAsync(id);
            if (manager == null) return NotFound();

            var properties = _context.Properties.Include(p => p.Manager).Where(p => p.Manager.Id == manager.Id);

            var managerProperties = await properties
                .Include(p => p.Ratings)
                .Include(p => p.PropertyType)
                .Include(p => p.Commodities)
                .ThenInclude(c => c.Commodity)
                .ToListAsync();

            var viewModel = new ManageOwnerDetailsViewModel()
            {
                Manager = manager,
                Properties = managerProperties
            };

            return View(viewModel);
        }
    }
}
