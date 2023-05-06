using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Data;
using FitnessApp.Models;
using Microsoft.AspNetCore.Identity;
using FitnessApp.DTOs;
using System.Globalization;
using FitnessApp.Utility;
using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.Controllers
{
    [Authorize]
    public class ExerciseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Person> _userManager;
        public ExerciseController(ApplicationDbContext context,
            UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Exercise
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Exercises
                .Include(e => e.Member)
                .ThenInclude(q => q.Person);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Exercise/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .Include(e => e.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // GET: Exercise/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exercise/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsertExerciseDTO insertExerciseDTO)
        {      
            
            if (ModelState.IsValid)
            {
                var res = (insertExerciseDTO.ExerciseTO - insertExerciseDTO.ExerciseFrom).TotalHours;
                double durationHours = double.Parse(res.ToString());
                TimeSpan duration = TimeSpan.FromHours(durationHours);
                var h = duration.Hours;
                var m = duration.Minutes;

                var exercise = new Exercise()
                {
                    Id = Guid.NewGuid().ToString(),
                    DateOfExercise = insertExerciseDTO.DateOfExercise,
                    ExerciseCount = insertExerciseDTO.ExerciseCount,
                    ExerciseFrom = insertExerciseDTO.ExerciseFrom,
                    ExerciseTO = insertExerciseDTO.ExerciseTO,
                    ExerciseDuration = duration.ToString("h\\:mm"),
                    MemberId = insertExerciseDTO.MemberId,
                    Title = insertExerciseDTO.Title,
                    // save the time like H:M without seconds 
                    ExerciseTimeFormat = ChangeDateFormat.ConvertTimeLocal(insertExerciseDTO.ExerciseFrom) 
                        + "-" + ChangeDateFormat.ConvertTimeLocal(insertExerciseDTO.ExerciseTO)
                };
                // check if the time that return to me has an hour ? if yes return time with hours Keyword
                exercise.ExerciseDuration = (h != 0 ? exercise.ExerciseDuration + " Hours" : m + " Minutes");
                _context.Exercises.Add(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(insertExerciseDTO);
        }

        // GET: Exercise/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .Include(q=>q.Member)
                .SingleOrDefaultAsync(q=>q.Id==id);

            var person = _context.Users
                .Where(q => q.Id == exercise.Member.PersonId)
                .Select(q => q.Email).FirstOrDefault();

            ViewBag.exercise = exercise;
            ViewBag.person = person;
            if (exercise == null)
            {
                return NotFound();
            }
            return View(exercise);
        }

        // POST: Exercise/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,  Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return NotFound();
            }
            var res = (exercise.ExerciseTO - exercise.ExerciseFrom).TotalHours;
            double durationHours = double.Parse(res.ToString());
            TimeSpan duration = TimeSpan.FromHours(durationHours);
            var m = duration.Minutes;
            var h = duration.Hours;
            if (ModelState.IsValid)
            {
                try
                {
                    exercise.ExerciseDuration = duration.ToString("h\\:mm");
                    exercise.ExerciseTimeFormat = ChangeDateFormat.ConvertTimeLocal(exercise.ExerciseFrom) 
                                     + "-" + ChangeDateFormat.ConvertTimeLocal(exercise.ExerciseTO);
                    exercise.ExerciseDuration = (h != 0 ? exercise.ExerciseDuration + " Hours" : m + " Minutes");

                    _context.Update(exercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(exercise.Id))
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
            return View(exercise);
        }

        // GET: Exercise/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .Include(e => e.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // POST: Exercise/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(string id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }


        public string ConvertTimeLocal(TimeSpan span)
        {
            var res = span.ToString("h\\:mm");
            string formattedTime = (DateTime.ParseExact(res, "H:mm", CultureInfo.InvariantCulture)
                .ToString("h:mm", CultureInfo.InvariantCulture).ToLowerInvariant());
            return formattedTime;
        }
    }
}
