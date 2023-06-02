using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Data;
using FitnessApp.Models;
using FitnessApp.DTOs;
using FitnessApp.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Person> _userManager;
        private readonly IWebHostEnvironment _hosting;
        public AdminController(ApplicationDbContext context, UserManager<Person> userManager, IWebHostEnvironment hosting)
        {
            _context = context;
            _userManager = userManager;
            _hosting = hosting;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Admins.Include(a => a.Person);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .Include(a => a.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsertAdminDTO insertAdminDTO)
        {
            if (ModelState.IsValid)
            {
                var person = new Person()
                {
                    Email = insertAdminDTO.Email,
                    Gender = insertAdminDTO.Gender,
                    EmailConfirmed = true,
                    RegisterDate = DateTime.Now,
                    PhoneNumber = "962789292164",
                    PhoneNumberConfirmed = true,
                    Role = Roles.Admin,
                    UserName = insertAdminDTO.Email,
                };
                // add user image
                UploadImage(person);

                // check user exist
                var check = await _userManager.FindByEmailAsync(insertAdminDTO.Email);
                if (check != null)
                {
                    ModelState.AddModelError("", "Email is already used");
                    return View(insertAdminDTO);
                }
                // add user
                var result = await _userManager.CreateAsync(person, insertAdminDTO.Password);
                var role = await _userManager.AddToRoleAsync(person, Roles.Admin);

                // add admin
                var admin = new Admin()
                {
                    Id = Guid.NewGuid().ToString(),
                    PersonId = person.Id
                };
                _context.Admins.Add(admin);
                await _context.SaveChangesAsync();
                SendRegisterMessage.RegisterAdminMessage(insertAdminDTO.Email
                    .Substring(0, insertAdminDTO.Email.IndexOf("@")),
                    insertAdminDTO.Email,insertAdminDTO.Password);
                return RedirectToAction(nameof(Index));

            }
            return View(insertAdminDTO);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", admin.PersonId);
            return View(admin);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,PersonId")] Admin admin)
        {
            if (id != admin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.Id))
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
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", admin.PersonId);
            return View(admin);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .Include(a => a.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var admin = await _context.Admins.FindAsync(id);
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(string id)
        {
            return _context.Admins.Any(e => e.Id == id);
        }

        private void UploadImage(Person model)
        {
            var file = HttpContext.Request.Form.Files;
            if (file.Count() > 0)
            {
                //@"wwwroot/"
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                var filestream = new FileStream(Path.Combine(_hosting.WebRootPath, "Images", ImageName), FileMode.Create);
                file[0].CopyTo(filestream);
                model.UserAvatar = ImageName;
            }
            else if (model.UserAvatar == null)
            {
                var userNickName = model.Email.Substring(0, model.Email.IndexOf("@"));
                model.UserAvatar = $"https://ui-avatars.com/api/?name={userNickName}";
            }
            else
            {
                model.UserAvatar = model.UserAvatar;
            }
        }







        public IActionResult AllUsers()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

    }
}
