using FitnessApp.Data;
using FitnessApp.DTOs;
using FitnessApp.Enums;
using FitnessApp.Models;
using FitnessApp.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace FitnessApp.Controllers
{
    public class APIController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Person> _userManager;
        public APIController(ApplicationDbContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public JsonResult GetTrainerMembers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
            if (user.Role == Roles.Trainer.ToString())
            {
                var trainer_members = _context.TrainersMembers
                    .Include(q => q.Trainer)
                    .ThenInclude(q => q.Person)
                    .Include(q => q.Member)
                    .ThenInclude(q => q.Person)
                    .Where(q => q.Trainer.PersonId == userId)
                    .ToList();
                return new JsonResult(trainer_members);
            }
            return new JsonResult("Please Login as Trainer To Access This Page");

        }


        public JsonResult GetAllTrainers()
        {
            
            var trainers = _context.Trainers
                .Include(q=>q.Person)
                 .AsNoTracking()
                 .ToList();

            return new JsonResult(trainers);
        }


        // home page
        public JsonResult GetMyMembers()
        {

            if (User.Identity.IsAuthenticated && User.IsInRole(Roles.Trainer))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();

                var membersBelongsToTrainer = _context.TrainersMembers
                    .Where(q => q.Trainer.PersonId == userId)
                    .Include(q => q.Member)
                    .ThenInclude(r => r.Person)
                    .Include(q => q.Trainer)
                    .ThenInclude(q => q.Person)
                    .ToList();
                return new JsonResult(membersBelongsToTrainer);
            }
            return new JsonResult("Please Sign as a Trainer TO Access To this page");

        }



        //public JsonResult GetListOfWeeklyDaysEnum()
        //{

        //    List<string> weeklyDays = new List<string>(Enum.GetValues(typeof(WeeklyDays))
        //        .Cast<WeeklyDays>()
        //        .Select(s => Enum.GetName(typeof(WeeklyDays), s)));
        //    return new JsonResult(weeklyDays);
        //}


        public JsonResult GetMemberExerciseDays()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
            List<DoExerciseDTO> exercise = new List<DoExerciseDTO>();
            if(user.Role == Roles.Member)
            {
                exercise = _context.Exercises
             .Where(q => q.Member.PersonId == user.Id)
             .Include(q => q.Member)
             .ThenInclude(q => q.Person)
             .Select(q => new DoExerciseDTO
             {
                 DayOfWeek = q.DateOfExercise.DayOfWeek.ToString(),
                 ExerciseCount = q.ExerciseCount,
                  ExerciseDuration =q.ExerciseDuration,
                  ExerciseTime = q.ExerciseTimeFormat,
                  ExerciseTitle = q.Title
             })
             .ToList();        
            }
            return new JsonResult(exercise);
        }

        public JsonResult GetMyNutirations()
        {
            
            return new JsonResult("");
        }



        public JsonResult GetAllMembersForInvoice()
        {

            List<Member> members1 = new List<Member>();

            var members = _context.Members
                .Include(q => q.Person)
                .Include(q => q.GymBundle)
                .AsNoTracking()
                .ToList();


            foreach (var item in members)
            {
                var invoice = _context.Invoices
                    .Include(q => q.Member)
                    .Include(q => q.Member.Person)
                    .Include(q => q.Member.GymBundle)
                    .SingleOrDefault(q => q.MemberId == item.Id);
                if (invoice != null)
                {
                    if (invoice.IsFullyPaid == false)
                    {
                        members1.Add(item);
                    }
                }
               else 
                {
                    members1.Add(item);
                }

            }





            return new JsonResult(members1);
        }


        public JsonResult GetMemberGymBundle(string idUser)
        {
            var invoice = _context.Invoices
                .Where(q => q.MemberId == idUser)
                .FirstOrDefault();

            var remaining = (invoice is null) ? 0 : invoice.RemainingValue;

            var userBundle = _context.Members
                .Include(q => q.GymBundle)
                .Include(q => q.Person)
                .Where(q => q.Id == idUser)
                .Select(q => new MemberInvoiceGet
                {
                    MemberId = q.Id,
                    TotalAmount = q.GymBundle.Price,
                    RemainingValue= remaining
                })
               .FirstOrDefault();





            return new JsonResult(userBundle);
        }


        public JsonResult GetAllGymBundle()
        {
            var gym = _context.GymBundles
                .AsNoTracking()
                .ToList();
            return new JsonResult(gym);
        }


        // ------ For My Nutation page



        public JsonResult GetMyNutraitonByParam(string filter)
        {
            var result = new List<Nutrition>();
            result = _context.Nutritions
            .Include(n => n.Member)
            .Include(q => q.Member.Person)
            .ToList();
            switch (filter.ToLower())
            {
                case "all":
                    result = _context.Nutritions
                        .Include(n => n.Member)
                        .Include(q => q.Member.Person)
                        .ToList();
                    break;

                case "today":
                    result = _context.Nutritions
                        .Include(n => n.Member)
                        .Include(q => q.Member.Person)
                        .Where(q=>q.DateOfNutrition.Date == DateTime.Now.Date)
                        .ToList();
                    break;

                case "finished":
                    result = _context.Nutritions
                        .Include(n => n.Member)
                        .Include(q => q.Member.Person)
                        .Where(q => q.DateOfNutrition < DateTime.Now)
                        .ToList();
                    break;
                case "active":
                    result = _context.Nutritions
                        .Include(n => n.Member)
                        .Include(q => q.Member.Person)
                        .Where(q => q.DateOfNutrition < DateTime.Now)
                        .ToList();
                    break;
                case "notstarted":
                    result = _context.Nutritions
                        .Include(n => n.Member)
                        .Include(q => q.Member.Person)
                        .Where(q => q.DateOfNutrition > DateTime.Now)
                        .ToList();
                    break;
            }


            return new JsonResult(result);
        }

        //public async Task<IActionResult> GetMoviesCalendar()
        //{
        //    List<MovieDTO> moviesDTO = new List<MovieDTO>();
        //    var movies = await _context.Movies.ToListAsync();
        //    movies.ForEach((item) =>
        //    {
        //        var mov = new MovieDTO
        //        {
        //            title = item.MovieName,
        //            start = item.MovieStartDate.Date, // Extract only the date part
        //            end = item.MovieEndDate.Date // Extract only the date part
        //        };
        //        moviesDTO.Add(mov);
        //    });

        //    string jsonDTO = JsonSerializer.Serialize(moviesDTO, new JsonSerializerOptions
        //    {
        //        WriteIndented = true,
        //        Converters = { new DateTimeConverterWithoutTime() } // Custom converter to remove time
        //    });

        //    return Content(jsonDTO, "application/json");
        //}



    }
}
