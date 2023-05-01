using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Data;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using FitnessApp.Utility;
using FitnessApp.DTOs;

namespace FitnessApp.Controllers
{
    public class RegistrationNotificationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GetCurrentUserProperties _getCurrentUserProperties;
        public RegistrationNotificationController(ApplicationDbContext context, 
            GetCurrentUserProperties getCurrentUserProperties)
        {
            _context = context;
            _getCurrentUserProperties = getCurrentUserProperties;
        }
        [Authorize(Roles =Roles.Admin)]
        // GET: RegistrationNotification
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.RegistrationNotifications
                .Include(r => r.GymBundle)
                .Include(a => a.Trainer)
                .ThenInclude(q => q.Person)
                .ToListAsync();
            return View(applicationDbContext);

            //var obj = await(from register in _context.RegistrationNotifications
            //           join gym in _context.GymBundles on register.GymBundleId equals gym.Id
            //           join trainer in _context.Trainers on register.TrainerId equals trainer.Id
            //           join person in _context.Users on trainer.PersonId equals person.Id
            //           select (new RegistrationNotificationsDTO 
            //           {
            //           Age = register.Age,
            //           Email =register.Email,
            //           Gender =register.Gender,
            //           GymBundleTitle = gym.BundleTitle,
            //           Height =register.Height,
            //           PhoneNumber =register.PhoneNumber,
            //           Trainer = person.Email,
            //           Weight=register.Weight
            //           })).ToListAsync();
            //        return View(obj);

        }

        // GET: RegistrationNotification/Create
        public IActionResult Create()
        {
            ViewData["GymBundleId"] = new SelectList(_context.GymBundles, "Id", "BundleTitle");
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "Id");
            return View();
        }

        // POST: RegistrationNotification/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( RegistrationNotification registrationNotification)
        {
            if (ModelState.IsValid)
            {
                var result = _getCurrentUserProperties.CheckUserExistance(registrationNotification.Email);
                if (result )
                {
                    ModelState.AddModelError("", "Sorry the user is exist ");
                    return View(registrationNotification);
                }
                registrationNotification.Id = Guid.NewGuid().ToString();
                
                _context.Add(registrationNotification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),"Home");
            }
            return View(registrationNotification);
        }











        //// GET: RegistrationNotification/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var registrationNotification = await _context.RegistrationNotifications
        //        .Include(r => r.GymBundle)
        //        .Include(r => r.Trainer)
        //        .Include(q => q.Trainer.Person)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (registrationNotification == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["GymBundleId"] = new SelectList(_context.GymBundles, "Id", "BundleTitle", registrationNotification.GymBundleId);
        //    ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "Peron.Email", registrationNotification.TrainerId);
        //    return View(registrationNotification);
        //}

        //// POST: RegistrationNotification/Edit/5
        //[Authorize(Roles = Roles.Admin)]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, RegistrationNotification registrationNotification)
        //{
        //    if (id != registrationNotification.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(registrationNotification);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!RegistrationNotificationExists(registrationNotification.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(registrationNotification);
        //}

        //// GET: RegistrationNotification/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var registrationNotification = await _context.RegistrationNotifications
        //        .Include(r => r.GymBundle)
        //        .Include(r => r.Trainer)
        //        .Include(q => q.Trainer.Person)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (registrationNotification == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(registrationNotification);
        //}

        //// POST: RegistrationNotification/Delete/5
        //[Authorize(Roles = Roles.Admin)]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var registrationNotification = await _context.RegistrationNotifications.FindAsync(id);
        //    _context.RegistrationNotifications.Remove(registrationNotification);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        //// GET: RegistrationNotification/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var registrationNotification = await _context.RegistrationNotifications
        //        .Include(r => r.GymBundle)
        //        .Include(r => r.Trainer)
        //       .Include(q => q.Trainer.Person)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (registrationNotification == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(registrationNotification);
        //}


        private bool RegistrationNotificationExists(string id)
        {
            return _context.RegistrationNotifications.Any(e => e.Id == id);
        }
    }
}
