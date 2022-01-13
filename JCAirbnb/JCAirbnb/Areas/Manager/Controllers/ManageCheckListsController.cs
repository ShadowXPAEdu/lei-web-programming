using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JCAirbnb.Data;
using JCAirbnb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using JCAirbnb.Areas.Manager.Models;

namespace JCAirbnb.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("{area}/{controller}/{action}/{id?}")]
    [Authorize(Roles = "Manager")]
    public class ManageCheckListsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ManageCheckListsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Manager/ManageCheckLists
        [Route("/{area}/{controller}/{id?}")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var checkLists = _context.CheckLists.Include(cl => cl.Company).Where(cl => cl.Company.Id == user.Id);

            return View(await checkLists.ToListAsync());
        }

        // GET: Manager/ManageCheckLists/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var checkList = await _context.CheckLists.Include(cl => cl.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (checkList == null || checkList.Company.Id != user.Id)
            {
                return NotFound();
            }

            return View(new Models.ManageCheckListDetailsViewModel()
            {
                CheckList = checkList,
                CheckListItems = await _context.CheckListItems.Include(cli => cli.CheckList).Where(cli => cli.CheckList.Id == id).ToListAsync()
            });
        }

        // GET: Manager/ManageCheckLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manager/ManageCheckLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title")] CheckList checkList)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                checkList.Id = Guid.NewGuid().ToString();
                checkList.Company = await _context.Companies.FindAsync(user.Id);
                _context.CheckLists.Add(checkList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(checkList);
        }

        // GET: Manager/ManageCheckLists/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var checkList = await _context.CheckLists.Include(cl => cl.Company).FirstOrDefaultAsync(cl => cl.Id == id);
            if (checkList == null || checkList.Company.Id != user.Id)
            {
                return NotFound();
            }
            return View(new ManageCheckListEditViewModel()
            {
                CheckList = checkList,
                CheckListItems = await _context.CheckListItems.Include(cli => cli.CheckList).Where(cli => cli.CheckList.Id == id).ToListAsync()
            });
        }

        // POST: Manager/ManageCheckLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CheckList")] ManageCheckListEditViewModel viewModel)
        {
            if (id != viewModel.CheckList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel.CheckList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckListExists(viewModel.CheckList.Id))
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
        public async Task<IActionResult> AddCheckListItem([Bind("CheckList,AddingItem,CheckListItemDescription,CheckListItemId")] ManageCheckListEditViewModel viewModel)
        {
            if (viewModel.AddingItem)
            {
                var checkListItem = new CheckListItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = viewModel.CheckListItemDescription,
                    Verified = false,
                    CheckList = await _context.CheckLists.FindAsync(viewModel.CheckList.Id)
                };

                _context.CheckListItems.Add(checkListItem);
                await _context.SaveChangesAsync();
            }
            else
            {
                var checkListItem = await _context.CheckListItems.FindAsync(viewModel.CheckListItemId);
                _context.CheckListItems.Remove(checkListItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Edit), new
            {
                viewModel.CheckList.Id
            });
        }

        // GET: Manager/ManageCheckLists/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var checkList = await _context.CheckLists.Include(cl => cl.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (checkList == null || checkList.Company.Id != user.Id)
            {
                return NotFound();
            }

            return View(checkList);
        }

        // POST: Manager/ManageCheckLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var checkList = await _context.CheckLists.FindAsync(id);
            _context.CheckLists.Remove(checkList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckListExists(string id)
        {
            return _context.CheckLists.Any(e => e.Id == id);
        }
    }
}
