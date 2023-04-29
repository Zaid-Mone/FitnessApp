using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Data;
using FitnessApp.Models;
using FitnessApp.Utility;
using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class GymBundleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GymBundleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GymBundle
        public async Task<IActionResult> Index()
        {
            return View(await _context.GymBundles.ToListAsync());
        }

        // GET: GymBundle/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymBundle = await _context.GymBundles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymBundle == null)
            {
                return NotFound();
            }

            return View(gymBundle);
        }

        // GET: GymBundle/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymBundle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( GymBundle gymBundle)
        {
            if (ModelState.IsValid)
            {
                gymBundle.Id = Guid.NewGuid().ToString();
                _context.Add(gymBundle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymBundle);
        }

        // GET: GymBundle/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymBundle = await _context.GymBundles.FindAsync(id);
            if (gymBundle == null)
            {
                return NotFound();
            }
            return View(gymBundle);
        }

        // POST: GymBundle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,BundleTitle,Price")] GymBundle gymBundle)
        {
            if (id != gymBundle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymBundle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymBundleExists(gymBundle.Id))
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
            return View(gymBundle);
        }

        // GET: GymBundle/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymBundle = await _context.GymBundles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymBundle == null)
            {
                return NotFound();
            }

            return View(gymBundle);
        }

        // POST: GymBundle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var gymBundle = await _context.GymBundles.FindAsync(id);
            _context.GymBundles.Remove(gymBundle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymBundleExists(string id)
        {
            return _context.GymBundles.Any(e => e.Id == id);
        }
    }
}
