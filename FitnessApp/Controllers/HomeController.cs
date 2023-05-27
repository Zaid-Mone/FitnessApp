using FitnessApp.Data;
using FitnessApp.Models;
using FitnessApp.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

    }
}
