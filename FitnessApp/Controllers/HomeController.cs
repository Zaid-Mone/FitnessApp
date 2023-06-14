using FitnessApp.Data;
using FitnessApp.DTOs;
using FitnessApp.Models;
using FitnessApp.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FitnessApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Person> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger,
            UserManager<Person> userManager,
            RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            if (_context.Users.Count() == 0 && _context.UserRoles.Count() == 0)
                Seeding();


            if(User.Identity.IsAuthenticated && User.IsInRole(Roles.Admin))
            {
                // Get Total Members
                var members = _userManager.GetUsersInRoleAsync(Roles.Member).GetAwaiter().GetResult().Count();
                ViewBag.Members = members;

                // Get Total Trainers
                var trainers = _userManager.GetUsersInRoleAsync(Roles.Trainer).GetAwaiter().GetResult().Count();
                ViewBag.Trainers = trainers;

                // Total Income
                var money = _context.Invoices.Sum(q => q.Userpays).ToString("C");
                ViewBag.Money = money;

                // Total Nutritions
                var nutration = _context.Nutritions.Count();
                ViewBag.Nutration = nutration;

                // rteturn the user that register in specific month 
                var usersByMonth = _context.Users
                .GroupBy(u => new { u.RegisterDate.Year, u.RegisterDate.Month })
                .Select(g => new { Month = g.Key.Month, Count = g.Count() })
                .OrderBy(g => g.Month)
                .ToList();

                var userCounts = new List<int>();

                for (int month = 1; month <= 12; month++)
                {
                    var userCount = usersByMonth.FirstOrDefault(g => g.Month == month)?.Count ?? 0;
                    userCounts.Add(userCount);
                }

                var serializedData = JsonConvert.SerializeObject(userCounts);
                ViewBag.usersCount= serializedData;


                // rteturn the member belong to Trainer 

                var trainerNames = _context.TrainersMembers
                    .Include(q=>q.Trainer)
                    .ThenInclude(q=>q.Person)
                    .Select(tm => tm.Trainer.Person.Email.Substring(0,tm.Trainer.Person.Email.IndexOf("@")))
                    .Distinct()
                    .ToList();
                //var trainersSerializedData1 = JsonConvert.SerializeObject(trainerNames);
                ViewBag.trainerCounts = trainerNames;

                 

                var trainerMemberCounts = _context.TrainersMembers
                        .GroupBy(tm => tm.MemberId)
                        .Select(g => new { TrainerID = g.Key, MemberCount = g.Count() })
                        .ToList();
                var memberCounts = trainerMemberCounts
                            .Select(tm => tm.MemberCount)
                            .ToList();


                var trainersSerializedData2 = JsonConvert.SerializeObject(memberCounts);
                ViewBag.trainerCounts2 = trainersSerializedData2;


            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public void Seeding()
        {
            AddRoles();
            var person = new Person()
            {
                Email = "Admin@email.com",
                Gender = Enums.Gender.Male,
                RegisterDate = DateTime.Now,
                UserName = "Admin@email.com",
                Role = Roles.Admin,
                PhoneNumber = "078781423",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var username = person.Email.Substring(0, person.Email.IndexOf("@"));
            person.UserAvatar = $"https://ui-avatars.com/api/?name={username}";
            _userManager.CreateAsync(person, "Admin1234*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(person, Roles.Admin).GetAwaiter().GetResult();

            var admin = new Admin()
            {
                Id = Guid.NewGuid().ToString(),
                PersonId = person.Id
            };
            _context.Admins.Add(admin);
            _context.SaveChanges();

            // Add Trainer
            var person1 = new Person()
            {
                Email = "Trainer@email.com",
                Gender = Enums.Gender.Male,
                RegisterDate = DateTime.Now,
                UserName = "Trainer@email.com",
                Role = Roles.Trainer,
                PhoneNumber = "078781423",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var username1 = person1.Email.Substring(0, person1.Email.IndexOf("@"));
            person1.UserAvatar = $"https://ui-avatars.com/api/?name={username}";
            _userManager.CreateAsync(person1, "Admin1234*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(person1, Roles.Trainer).GetAwaiter().GetResult();

            var trainer = new Trainer()
            {
                Id = Guid.NewGuid().ToString(),
                PersonId = person1.Id
            };
            _context.Trainers.Add(trainer);
            _context.SaveChanges();

            // Add Trainer
            var person2 = new Person()
            {
                Email = "Member@email.com",
                Gender = Enums.Gender.Male,
                RegisterDate = DateTime.Now,
                UserName = "Member@email.com",
                Role = Roles.Member,
                PhoneNumber = "078781423",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var username2 = person2.Email.Substring(0, person2.Email.IndexOf("@"));
            person2.UserAvatar = $"https://ui-avatars.com/api/?name={username}";
            _userManager.CreateAsync(person2, "Admin1234*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(person2, Roles.Member).GetAwaiter().GetResult();

            var member = new Member()
            {
                Id = Guid.NewGuid().ToString(),
                PersonId = person1.Id,
                Age=23,
                DateOfBirth= DateTime.Now,
                GymBundleId= "66efd204-d497-4c80-baf5-8a0500e167df",
                Height=180,
                MembershipFrom=DateTime.Now,
                MembershipTo=DateTime.Now.AddDays(30),
                Weight=100,
                
            };
            _context.Members.Add(member);
            _context.SaveChanges();

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
             _context.SaveChanges();

            var trainersss = _context.Trainers.Where(q => q.Id == trainer.Id).FirstOrDefault();

            // add Trainers Members
            var trainermember = new TrainersMember()
            {
                Id = Guid.NewGuid().ToString(),
                MemberId = member.Id,
                TrainerId = trainer.Id,
            };
            _context.TrainersMembers.Add(trainermember);
             _context.SaveChanges();

        }
        public void AddRoles()
        {
            if (!_roleManager.RoleExistsAsync(Roles.Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin }).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole { Name = Roles.Member }).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole { Name = Roles.Trainer }).GetAwaiter().GetResult();

            }
        }

        // Called Stored Procedure
        //public void Geto()
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var userIdParameter = new SqlParameter("@UserId", userId);
        //    var persons = _context.Users.FromSqlRaw("EXEC GetUserWithID @UserId", userIdParameter).ToList();
        //}
    }
}
