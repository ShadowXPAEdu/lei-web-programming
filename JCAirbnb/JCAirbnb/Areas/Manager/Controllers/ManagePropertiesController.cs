using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JCAirbnb.Data;
using JCAirbnb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using JCAirbnb.Areas.Manager.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

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
            var manager = (await _userManager.GetUserAsync(User));
            var aux = _context.Properties.Where(p => p.Manager.Id == manager.Id);
            aux = aux.Include(p => p.Divisions).Include(p => p.Commodities).ThenInclude(c => c.Commodity).Include(p => p.PropertyType);
            return View(await aux.ToListAsync());
        }

        // GET: Manager/ManageProperties/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var @property = await _context.Properties.
                Include(p => p.Divisions).Include(p => p.Commodities).ThenInclude(c => c.Commodity).Include(p => p.PropertyType)
                .Include(p => p.Reviews).ThenInclude(r => r.User).Include(p => p.Ratings)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@property == null) return NotFound();

            return View(@property);
        }

        // GET: Manager/ManageProperties/Create
        public IActionResult Create()
        {
            ManagePropertyCreateViewModel managePropertyCreateViewModel = new ManagePropertyCreateViewModel();
            managePropertyCreateViewModel.PropertyTypes = _context.PropertyTypes.ToList();
            return View(managePropertyCreateViewModel);
        }

        // POST: Manager/ManageProperties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Property")] ManagePropertyCreateViewModel viewModel,
        [FromForm(Name = "guest")] int guest, [FromForm(Name = "bedroom")] int bedroom, [FromForm(Name = "bed")] int bed,
        [FromForm(Name = "bath")] int bath, [FromForm(Name = "privateBath")] int privateBath,
        [FromForm(Name = "propTypeId")] string propTypeId)
        {
            if (ModelState.IsValid)
            {
                viewModel.Property.Id = Guid.NewGuid().ToString();
                viewModel.Property.Manager = (await _userManager.GetUserAsync(User)); //falta por o manager associado

                viewModel.Property.Divisions = new()
                {
                    Id = viewModel.Property.Id,
                    Guest = guest,
                    Bedroom = bedroom,
                    Bed = bed,
                    Bath = bath,
                    PrivateBath = privateBath
                };

                viewModel.Property.Ratings = new()
                {
                    Id = viewModel.Property.Id
                };

                var propType = _context.PropertyTypes.FirstOrDefault(pt => pt.Id == propTypeId);
                viewModel.Property.PropertyType = propType;

                _context.Add(viewModel.Property);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UploadPhotos(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Images"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UploadPhotos([FromForm(Name = "ImageData")] string ImageData)
        //{
        //    var str = ImageData;
        //    HttpPostedFileBase file = Request.Files(ImageData);
        //    //ContentRepository service = new ContentRepository();
        //    //int i = service.UploadImageInDataBase(file, model);
        //    //if (i == 1)
        //    //{
        //    //    return RedirectToAction("Index");
        //    //}
        //    Image image = Image.FromFile(@"~\Content\img\toendra.JPG");
        //    return View();
        //}

        //public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        //{
        //    long size = files.Sum(f => f.Length);

        //    foreach (var formFile in files)
        //    {
        //        if (formFile.Length > 0)
        //        {
        //            var filePath = Path.GetTempFileName();

        //            using (var stream = System.IO.File.Create(filePath))
        //            {
        //                await formFile.CopyToAsync(stream);
        //            }
        //        }
        //    }

        //    // Process uploaded files
        //    // Don't rely on or trust the FileName property without validation.

        //    return Ok(new { count = files.Count, size });
        //}

        //public ActionResult FileUpload(HttpPostedFileBase file)
        //{
        //    if (file != null)
        //    {
        //        string pic = System.IO.Path.GetFileName(file.FileName);
        //        string path = System.IO.Path.Combine(
        //                               Server.MapPath("~/images/profile"), pic);
        //        // file is uploaded
        //        file.SaveAs(path);

        //        // save the image path path to the database or you can send image 
        //        // directly to database
        //        // in-case if you want to store byte[] ie. for DB
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            file.InputStream.CopyTo(ms);
        //            byte[] array = ms.GetBuffer();
        //        }

        //    }
        //    // after successfully uploading redirect the user
        //    return RedirectToAction("actionname", "controller name");
        //}

        // GET: Manager/ManageProperties/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var property = await _context.Properties
                .Include(p => p.Divisions)
                .Include(p => p.Commodities)
                .ThenInclude(c => c.Commodity)
                .Include(pt => pt.PropertyType)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null) return NotFound();

            var manager = (await _userManager.GetUserAsync(User));

            if (manager.Id != property.Manager.Id) return NotFound();

            return View(new ManagePropertyEditViewModel()
            {
                PropertyTypes = await _context.PropertyTypes.ToListAsync(),
                Property = property,
                Commodities = await _context.Commodities.ToListAsync()
            });
        }

        // POST: Manager/ManageProperties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Property,Commodities,AddingCommodity")] ManagePropertyEditViewModel viewModel,
            [FromForm(Name = "guest")] int guest, [FromForm(Name = "bedroom")] int bedroom, [FromForm(Name = "bed")] int bed,
        [FromForm(Name = "bath")] int bath, [FromForm(Name = "privateBath")] int privateBath, [FromForm(Name = "propTypeId")] string propTypeId)
        {
            if (id != viewModel.Property.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    viewModel.Property.Divisions.Guest = guest;
                    viewModel.Property.Divisions.Bedroom = bedroom;
                    viewModel.Property.Divisions.Bed = bed;
                    viewModel.Property.Divisions.Bath = bath;
                    viewModel.Property.Divisions.PrivateBath = privateBath;

                    var propType = _context.PropertyTypes.FirstOrDefault(pt => pt.Id == propTypeId);
                    viewModel.Property.PropertyType = propType;
                    _context.Update(viewModel.Property);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(viewModel.Property.Id))
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
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCommodity(string id, [Bind("Property,Commodities,AddingCommodity")] ManagePropertyEditViewModel viewModel,
            [FromForm(Name = "commodity")] string commodity, string submit)
        {
            if (ModelState.IsValid)
            {
                if (submit == "Included Commodity")
                {
                    var commo = await _context.Commodities.FirstOrDefaultAsync(c => c.Id == commodity);
                    if (commo == null)
                    {
                        return NotFound();
                    }
                    var prop = await _context.Properties.Include(p => p.Commodities).ThenInclude(c => c.Commodity).FirstOrDefaultAsync(p => p.Id == id);
                    if (prop == null)
                    {
                        return NotFound();
                    }

                    PropertyCommodity propertyCommodity = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Commodity = commo,
                        Included = true
                    };

                    if (!prop.Commodities.Contains(propertyCommodity))
                    {
                        prop.Commodities.Add(propertyCommodity);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Edit", new { id });
                    }

                    return RedirectToAction("Edit", new { id });
                }
                else
                {
                    var commo = _context.Commodities.FirstOrDefault(c => c.Id == commodity);
                    if (commo == null)
                    {
                        return NotFound();
                    }
                    var prop = _context.Properties.Include(p => p.Commodities).ThenInclude(c => c.Commodity).FirstOrDefault(p => p.Id == id);
                    if (prop == null)
                    {
                        return NotFound();
                    }

                    PropertyCommodity propertyCommodity = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Commodity = commo,
                        Included = false
                    };

                    if (!prop.Commodities.Contains(propertyCommodity))
                    {
                        //    if (!_context.Properties.FirstOrDefault(p => p.Id == id).Commodities.Contains(propertyCommodity))
                        //{
                        prop.Commodities.Add(propertyCommodity);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Edit", new { id });
                    }

                    return RedirectToAction("Edit", new { id });

                }
            }
            return RedirectToAction("Edit", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCommodity(string id, [Bind("Property,Commodities,AddingCommodity")] ManagePropertyEditViewModel viewModel,
            [FromForm(Name = "commodity")] string commodity)
        {
            var prop = await _context.Properties.Include(p => p.Commodities).ThenInclude(c => c.Commodity).FirstOrDefaultAsync(p => p.Id == id);
            if (prop == null)
            {
                return NotFound();
            }
            //var commo = _context.Commodities.FirstOrDefault(c => c.Id == commodity);
            //if (commo == null)
            //{
            //    return NotFound();
            //}

            //var com = _context.Properties.FirstOrDefault(p => p.Id == id).Commodities.Find(c => c.Id == commodity);
            ////var com = prop.Commodities.FirstOrDefault(c => c.Commodity.Id == commodity);
            //if (com == null)
            //{
            //    return NotFound();
            //}

            var propCom = await _context.PropertyCommodities.Include(pc => pc.Commodity).FirstOrDefaultAsync(pc => pc.Commodity.Id == commodity);
            if (propCom == null)
            {
                return NotFound();
            }

            if (prop.Commodities.Contains(propCom))
            {
                prop.Commodities.Remove(propCom);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new { id });
            }
            return RedirectToAction("Edit", new { id });
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
            //first delete all PropertyCommodities with the same ID
            var @property = await _context.Properties.Include(p => p.Commodities)
                .ThenInclude(c => c.Commodity).FirstOrDefaultAsync(p => p.Id == id);
            //@property.Commodities.RemoveAll(c => c.Id = id);
            property.Commodities.Clear();
            //await _context.SaveChangesAsync();
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
