using JCAirbnb.Data;
using JCAirbnb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using JCAirbnb.Models.ViewModel;

namespace JCAirbnb.Controllers
{
    [Route("{action}/{id?}")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        private const int DEFAULT_ITEMS_PER_PAGE = 50;

        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("/")]
        public async Task<IActionResult> Index(int? page, string q)
        {
            int pg = page ?? 1;
            pg = pg < 1 ? 1 : pg;
            q ??= "";

            var properties = _context.Properties.Include(p => p.PropertyType)
                .Include(p => p.Ratings)
                .OrderByDescending(p => (p.Ratings.Cleanliness + p.Ratings.Accuracy + p.Ratings.Communication + p.Ratings.Location + p.Ratings.CheckIn + p.Ratings.Value) / 6.0f)
                .Skip((pg - 1) * DEFAULT_ITEMS_PER_PAGE)
                .Take(DEFAULT_ITEMS_PER_PAGE)
                .Where(p => p.Title.Contains(q) || p.Location.Contains(q) || p.PropertyType.Title.Contains(q));

            var count = _context.Properties.Count();

            var viewModel = new HomeViewModel()
            {
                Properties = await properties.Include(p => p.Photos).Include(p => p.Manager).ToListAsync(),
                Page = pg,
                HasPrevPage = pg != 1,
                HasNextPage = count > DEFAULT_ITEMS_PER_PAGE * (pg)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Property(string id)
        {
            if (id == null) return NotFound();

            var property = await _context.Properties
                .Include(p => p.Manager)
                .Include(p => p.Divisions)
                .Include(p => p.PropertyType)
                .Include(p => p.Ratings)
                .Include(p => p.Photos)
                .Include(p => p.Commodities)
                .ThenInclude(c => c.Commodity)
                .Include(p => p.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null) return NotFound();

            return View(property);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
