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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.IO;
using FitnessApp.Utility;
using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class TrainerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Person> _userManager;
        private readonly IWebHostEnvironment _hosting;
        public TrainerController(ApplicationDbContext context,
            UserManager<Person> userManager, IWebHostEnvironment hosting)
        {
            _context = context;
            _userManager = userManager;
            _hosting = hosting;
        }


        // GET: Trainer
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Trainers.Include(t => t.Person);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Trainer/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .Include(t => t.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // GET: Trainer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trainer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsertTrainerDTO insertTrainersDTO)
        {
            if (ModelState.IsValid)
            {
                var person = new Person()
                {
                    Email = insertTrainersDTO.Email,
                    Gender = insertTrainersDTO.Gender,
                    EmailConfirmed = true,
                    RegisterDate = DateTime.Now,
                    PhoneNumber = insertTrainersDTO.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    Role = Roles.Trainer,
                    UserName = insertTrainersDTO.Email,
                };
                // add image to user
                UploadImage(person);
                // check if the user exist
                var check = await _userManager.FindByEmailAsync(insertTrainersDTO.Email);
                if (check != null)
                {
                    ModelState.AddModelError("", "Email is already used");
                    return View(insertTrainersDTO);
                }
                // add to data to Users object and Users Table
                var result = await _userManager.CreateAsync(person, insertTrainersDTO.Password);
                if(result.Succeeded)
                {
                    // add role to the user
                    var role = await _userManager.AddToRoleAsync(person, Roles.Trainer);
                }

                // add to data to trainer object and Trainer Table
                var trainer = new Trainer()
                {
                    Id = Guid.NewGuid().ToString(),
                    PersonId = person.Id
                };
                _context.Trainers.Add(trainer);
                await _context.SaveChangesAsync();
                SendRegisterMessage.RegisterTrainerMessage(insertTrainersDTO.Email.Substring(0,insertTrainersDTO.Email.IndexOf("@")), insertTrainersDTO.Email, insertTrainersDTO.Password);
                return RedirectToAction(nameof(Index));
            }
            return View(insertTrainersDTO);
        }

        // GET: Trainer/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }
            return View(trainer);
        }

        // POST: Trainer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Trainer trainer)
        {
            if (id != trainer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerExists(trainer.Id))
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
            return View(trainer);
        }

        // GET: Trainer/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .Include(t => t.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // POST: Trainer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerExists(string id)
        {
            return _context.Trainers.Any(e => e.Id == id);
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
    }
}
