using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Data;
using FitnessApp.Models;

namespace FitnessApp.Controllers
{
    public class MembersssssController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembersssssController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Membersssss
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Members.Include(m => m.GymBundle).Include(m => m.Person).Include(m => m.Trainer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Membersssss/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.GymBundle)
                .Include(m => m.Person)
                .Include(m => m.Trainer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Membersssss/Create
        public IActionResult Create()
        {
            ViewData["GymBundleId"] = new SelectList(_context.GymBundles, "Id", "Id");
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "Id");
            return View();
        }

        // POST: Membersssss/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PersonId,Age,Weight,Height,DateOfBirth,GymBundleId,ExpectedWeight,BMIStatus,IsMemberOverWeight,MembershipFrom,MembershipTo,TrainerId")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GymBundleId"] = new SelectList(_context.GymBundles, "Id", "Id", member.GymBundleId);
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", member.PersonId);
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "Id", member.TrainerId);
            return View(member);
        }

        // GET: Membersssss/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["GymBundleId"] = new SelectList(_context.GymBundles, "Id", "Id", member.GymBundleId);
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", member.PersonId);
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "Id", member.TrainerId);
            return View(member);
        }

        // POST: Membersssss/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,PersonId,Age,Weight,Height,DateOfBirth,GymBundleId,ExpectedWeight,BMIStatus,IsMemberOverWeight,MembershipFrom,MembershipTo,TrainerId")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
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
            ViewData["GymBundleId"] = new SelectList(_context.GymBundles, "Id", "Id", member.GymBundleId);
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", member.PersonId);
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "Id", member.TrainerId);
            return View(member);
        }

        // GET: Membersssss/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.GymBundle)
                .Include(m => m.Person)
                .Include(m => m.Trainer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Membersssss/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var member = await _context.Members.FindAsync(id);
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(string id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
