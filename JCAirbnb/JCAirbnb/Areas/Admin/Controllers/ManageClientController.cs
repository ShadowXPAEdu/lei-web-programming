﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/{controller}/{action}/{id?}")]
    [Authorize(Roles = "Administrator")]
    public class ManageClientController : Controller
    {
        [Route("/{area}/{controller}/{id?}")]
        public IActionResult Index(int? id)
        {
            return View(id);
        }
    }
}
