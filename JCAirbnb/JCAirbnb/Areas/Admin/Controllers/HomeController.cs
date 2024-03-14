using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JCAirbnb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/{action}/{id?}")]
    [Authorize(Roles = "Administrator")]
    public class HomeController : Controller
    {
        [Route("/{area}/{id?}")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
