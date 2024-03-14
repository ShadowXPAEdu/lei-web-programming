using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("{area}/{action}/{id?}")]
    [Authorize(Roles = "Administrator, Manager")] // Admins can manage stuff too?
    public class HomeController : Controller
    {
        [Route("/{area}/{id?}")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
