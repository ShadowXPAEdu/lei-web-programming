using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JCAirbnb.Data;
using JCAirbnb.Models;
using Microsoft.AspNetCore.Authorization;
using JCAirbnb.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;

namespace JCAirbnb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/{controller}/{action}/{id?}")]
    [Authorize(Roles = "Administrator")]
    public class ManageClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManageClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ManageClients
        [Route("/{area}/{controller}/{id?}")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Clients.Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/ManageClients/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Admin/ManageClients/Edit/5
        // TODO: edit user roles
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            //ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", client.Id);
            return View(new ManageClientEditViewModel()
            {
                Roles = new(_context.Roles),
                Client = client
            });
        }

        // POST: Admin/ManageClients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Roles,Client,AddingRole")] ManageClientEditViewModel viewModel,
            [FromForm(Name = "role")] string role)
        {
            if (id != viewModel.Client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (viewModel.AddingRole)
                {
                    // Check if already has role
                    // Role is an ID here
                    var userRole = new IdentityUserRole<string>()
                    {
                        RoleId = role,
                        UserId = viewModel.Client.Id
                    };
                    if (!await _context.UserRoles.ContainsAsync(userRole))
                    {
                        await _context.UserRoles.AddAsync(new()
                        {
                            RoleId = role,
                            UserId = viewModel.Client.Id
                        });
                        await _context.SaveChangesAsync();
                    }
                    return await Edit(id);
                }
                else
                {
                    // Remove role
                    // Role is the name
                    var roleId = (await _context.Roles.FirstOrDefaultAsync(r => r.Name == role))?.Id;
                    var userRole = await _context.UserRoles.FirstOrDefaultAsync(r => r.RoleId == roleId && r.UserId == viewModel.Client.Id);
                    _context.UserRoles.Remove(userRole);
                    await _context.SaveChangesAsync();
                    return await Edit(id);
                }

                //try
                //{
                //    _context.Update(viewModel.Client);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!ClientExists(viewModel.Client.Id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                //return RedirectToAction(nameof(Index));
            }
            //ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", client.Id);
            return await Edit(id);
        }

        // GET: Admin/ManageClients/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Admin/ManageClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(string id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
