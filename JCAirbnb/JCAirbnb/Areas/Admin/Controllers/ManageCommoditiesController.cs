using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class ManageCommoditiesController : Controller
    {
        // GET: ManageCommoditiesController
        [Route("/{area}/{controller}")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: ManageCommoditiesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ManageCommoditiesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageCommoditiesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ManageCommoditiesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManageCommoditiesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ManageCommoditiesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManageCommoditiesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
