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
using Microsoft.AspNetCore.Authorization;
using FitnessApp.Utility;
using System.Security.Claims;

namespace FitnessApp.Controllers
{
    [Authorize(Roles = Roles.Trainer)]
    public class NutritionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NutritionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Nutrition
        public async Task<IActionResult> Index()
        {
            var personId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var trainer = _context.Trainers.Include(q => q.Person).SingleOrDefault(q => q.PersonId == personId);
            var applicationDbContext = await _context.Nutritions
                .Include(n => n.Member)
                .Include(q => q.Member.Person)
                .Where(q=>q.TrainerId == trainer.Id).ToListAsync();
            return View( applicationDbContext);
        }

        // GET: Nutrition/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nutrition = await _context.Nutritions
                .Include(n => n.Member)
                .Include(q => q.Member.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nutrition == null)
            {
                return NotFound();
            }

            return View(nutrition);
        }

        // GET: Nutrition/Create
        public IActionResult Create(string id)
        {
            return View();
        }

        // POST: Nutrition/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsertNutrationDTO insertNutrationDTO)
        {
            if (ModelState.IsValid)
            {
                var personId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var trainer = _context.Trainers.Include(q=>q.Person).SingleOrDefault(q => q.PersonId == personId);

                var member = _context.Members.Where(q => q.Id == insertNutrationDTO.MemberId).FirstOrDefault();
                var nut = new Nutrition()
                {
                    Id = Guid.NewGuid().ToString(),
                    MealName = insertNutrationDTO.MealName,
                    MemberId = insertNutrationDTO.MemberId,
                    DateOfNutrition = insertNutrationDTO.DateOfNutrition,
                    NameOfDay = insertNutrationDTO.DateOfNutrition.DayOfWeek.ToString()
                     + "-" +
                     insertNutrationDTO.DateOfNutrition.Year.ToString(),
                    MealType = insertNutrationDTO.MealType,
                    TrainerId = trainer.Id
                };
                _context.Nutritions.Add(nut);
                await _context.SaveChangesAsync();



                return RedirectToAction(nameof(Index));
            }
            return View(insertNutrationDTO);
        }

        // GET: Nutrition/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nutrition = await _context.Nutritions
                .Include(q => q.Member)
                .ThenInclude(q=>q.Person)
                .Include(q=>q.Trainer)
                .SingleOrDefaultAsync(q => q.Id == id);
            var memberId = _context.Members.Where(q => q.Id == nutrition.MemberId).Select(q => q.Id).FirstOrDefault();
            ViewBag.memberId = memberId;

            if (nutrition == null)
            {
                return NotFound();
            }
            return View(nutrition);
        }

        // POST: Nutrition/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Nutrition nutrition)
        {
            if (id != nutrition.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var personId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var trainer = _context.Trainers.Include(q => q.Person).SingleOrDefault(q => q.PersonId == personId);
                    nutrition.NameOfDay = nutrition.DateOfNutrition.DayOfWeek.ToString()
                     + "-" +
                     nutrition.DateOfNutrition.Year.ToString();
                   nutrition.TrainerId = trainer.Id; 
                    _context.Update(nutrition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NutritionExists(nutrition.Id))
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
            var nut = await _context.Nutritions.Include(q => q.Member).SingleOrDefaultAsync(q => q.Id == id);
            var memberId = _context.Members.Where(q => q.Id == nut.MemberId).Select(q => q.Id).FirstOrDefault();
            ViewBag.memberId = memberId;
            return View(nutrition);
        }
        // GET: Nutrition/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nutrition = await _context.Nutritions
                .Include(n => n.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nutrition == null)
            {
                return NotFound();
            }

            return View(nutrition);
        }

        // POST: Nutrition/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var nutrition = await _context.Nutritions.FindAsync(id);
            _context.Nutritions.Remove(nutrition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NutritionExists(string id)
        {
            return _context.Nutritions.Any(e => e.Id == id);
        }

    }
}
