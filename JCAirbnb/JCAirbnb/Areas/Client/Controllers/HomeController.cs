using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("{area}/{controller}/{action}/{id?}")]
    [Authorize]
    public class HomeController : Controller
    {
        [Route("/{area}/{controller}/{id?}")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
