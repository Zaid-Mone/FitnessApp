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
using Microsoft.AspNetCore.Hosting;
using FitnessApp.DTOs;
using FitnessApp.Utility;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Person> _userManager;
        private readonly IWebHostEnvironment _hosting;
        public MemberController(ApplicationDbContext context,
            UserManager<Person> userManager,
            IWebHostEnvironment hosting)
        {
            _context = context;
            _userManager = userManager;
            _hosting = hosting;
        }

        // GET: Member
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.Members
                .Include(m => m.GymBundle)
                .Include(q => q.Person)
                .Include(q=>q.Trainer)
                .Include(q=>q.Trainer.Person)
                .ToListAsync();
            return View(applicationDbContext);
        }

        // GET: Member/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                 .Include(m => m.GymBundle)
                .Include(q => q.TrainersMembers)
                .ThenInclude(q => q.Trainer)
                .Include(q => q.TrainersMembers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Member/Create
        public IActionResult Create()
        {
            ViewData["GymBundleId"] = new SelectList(_context.GymBundles, "Id", "BundleTitle");
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsertMemberDTO insertMemberDTO)
        {
            if (ModelState.IsValid)
            {
                var person = new Person()
                {
                    Email = insertMemberDTO.Email,
                    Gender = insertMemberDTO.Gender,
                    EmailConfirmed = true,
                    RegisterDate = DateTime.Now,
                    PhoneNumber = "9627892921641",
                    PhoneNumberConfirmed = true,
                    Role = Roles.Member,
                    UserName = insertMemberDTO.Email,
                };

                // add user image
                UploadImage(person);

                // check user exist
                var check = await _userManager.FindByEmailAsync(insertMemberDTO.Email);
                if (check != null)
                {
                    ModelState.AddModelError("", "Email is already used");
                    return View(insertMemberDTO);
                }
                // add user
                var result = await _userManager.CreateAsync(person, insertMemberDTO.Password);
                var role = await _userManager.AddToRoleAsync(person, Roles.Member);

                // add admin
                var member = new Member()
                {
                    Id = Guid.NewGuid().ToString(),
                    Age = insertMemberDTO.Age,
                    DateOfBirth = insertMemberDTO.DateOfBirth,
                    GymBundleId = insertMemberDTO.GymBundleId,
                    Height = insertMemberDTO.Height,
                    Weight = insertMemberDTO.Weight,
                    PersonId = person.Id,
                    TrainerId=insertMemberDTO.TrainerId,
                };
                var memeberDTO = new WeightCalculateDTO
                {
                    Weight = member.Weight,
                    Height = member.Height,
                    Age = member.Age
                };
                member.BMIStatus = WeightState.SetWeightStatus(member.Height, member.Weight);
                member.IsMemberOverWeight = CalculateWeight.IsOverweight(memeberDTO);
                member.ExpectedWeight = CalculateWeight.GetPerfectWeight(memeberDTO);
                // find Gym bundle with number of days.
                var gym = _context.GymBundles.Where(q => q.Id == member.GymBundleId).FirstOrDefault();
                var date = DateTime.Now;
                member.MembershipFrom = date;
                member.MembershipTo = member.MembershipFrom.AddDays((double)gym.NumberOfDays);
                _context.Members.Add(member);
                await _context.SaveChangesAsync();


                // find the traner
                var trainer = _context.Trainers.Where(q => q.Id == insertMemberDTO.TrainerId).FirstOrDefault();

                // add Trainers Members
                var trainermember = new TrainersMember()
                {
                    Id = Guid.NewGuid().ToString(),
                    MemberId = member.Id,
                    TrainerId = trainer.Id,
                };
                _context.TrainersMembers.Add(trainermember);
                await _context.SaveChangesAsync();
                // send SMS Message
                SendRegisterMessage.RegisterMemberMessage(insertMemberDTO.Email.Substring(0, insertMemberDTO.Email.IndexOf("@")), insertMemberDTO.Email, insertMemberDTO.Password);
                return RedirectToAction(nameof(Index));
            }
            ViewData["GymBundleId"] = new SelectList(_context.GymBundles, "Id", "BundleTitle", insertMemberDTO.GymBundleId);
            ViewBag.msg = false;
            return View(insertMemberDTO);
        }

        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(q=>q.Person)
                .Include(q=>q.GymBundle)
                .Include(q=>q.Trainer)
                .SingleOrDefaultAsync(q=>q.Id==id);
            if (member == null)
            {
                return NotFound();
            }
            var updatemember = new UpdateMember
            {
                PersonId = member.PersonId,
                GymBundleId = member.GymBundleId,
                MemberId = member.Id,
                Age = member.Age,
                DateOfBirth = member.DateOfBirth,
                Height = member.Height,
                Weight = member.Weight,
                PersonEmail = member.Person.Email,
                MembershipFrom = member.MembershipFrom,
                MembershipTo = member.MembershipTo,
                TrainerId = member.TrainerId,                
            };
            var trainers = await _context.Trainers
            .Include(t => t.Person) // Include the related Person table
            .ToListAsync();

            ViewData["TrainerId"] = new SelectList(trainers, "Id", "Person.Name", member.TrainerId);

            ViewData["GymBundleId"] = new SelectList(_context.GymBundles, "Id", "BundleTitle", member.GymBundleId);
            ViewData["TrainerId"] = new SelectList(trainers, "Id", "Person.Email", member.TrainerId);
            return View(updatemember);
        }

        // POST: Member/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateMember updateMember)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<TrainersMember> trainermember = _context.TrainersMembers.Where(q => q.MemberId == updateMember.MemberId).ToList();
                    foreach (var item in trainermember)
                    {
                        _context.TrainersMembers.Remove(item);
                    }
                    await _context.SaveChangesAsync();
                    var memeberDTO = new WeightCalculateDTO
                    {
                        Weight = updateMember.Weight,
                        Height = updateMember.Height,
                        Age = updateMember.Age
                    };
                    _context.Members.Update(new Member
                    {
                        Id = updateMember.MemberId,
                        PersonId = updateMember.PersonId,
                        GymBundleId= updateMember.GymBundleId,
                        Height= updateMember.Height,
                        Weight= updateMember.Weight,
                        Age= updateMember.Age,
                        DateOfBirth = updateMember.DateOfBirth,
                        MembershipTo = updateMember.MembershipTo,
                        MembershipFrom= updateMember.MembershipFrom,
                        BMIStatus = WeightState.SetWeightStatus(memeberDTO.Height, memeberDTO.Weight),
                        ExpectedWeight = CalculateWeight.GetPerfectWeight(memeberDTO),
                        IsMemberOverWeight = CalculateWeight.IsOverweight(memeberDTO),
                        TrainerId = updateMember.TrainerId,
                    });
                    await _context.SaveChangesAsync();

                    _context.TrainersMembers.Add(new TrainersMember
                    {
                        Id =  Guid.NewGuid().ToString(),
                        MemberId = updateMember.MemberId,
                        TrainerId = updateMember.TrainerId,
                    });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(updateMember.MemberId))
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
            ViewData["GymBundleId"] = new SelectList(_context.GymBundles, "Id", "BundleTitle", updateMember.GymBundleId);
            ViewData["TrainerId"] = new SelectList(_context.GymBundles, "Id", "Email", updateMember.TrainerId);
            return View(updateMember);
        }

        // GET: Member/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
           List<Invoice> inv =  _context.Invoices.Where(q => q.MemberId == id).ToList();
            if(inv != null)
            {
                foreach (var item in inv)
                {
                    _context.Invoices.Remove(item);
                }
                _context.SaveChanges();
            }

            var member = await _context.Members
                .Include(m => m.GymBundle)
                .Include(q => q.TrainersMembers)
                .ThenInclude(q => q.Trainer)
                .Include(q => q.TrainersMembers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Member/Delete/5
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
