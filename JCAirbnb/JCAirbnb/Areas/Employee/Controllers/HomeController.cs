using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Route("{area}/{action}/{id?}")]
    [Authorize(Roles = "Manager,Employee")]
    public class HomeController : Controller
    {
        [Route("/{area}/{id?}")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
