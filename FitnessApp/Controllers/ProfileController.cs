using FitnessApp.Data;
using FitnessApp.DTOs;
using FitnessApp.Models;
using FitnessApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FitnessApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Person> _userManager;
        private readonly GetCurrentUserProperties _getCurrentUserProperties;
        public ProfileController(ApplicationDbContext context, 
            UserManager<Person> userManager, 
            GetCurrentUserProperties getCurrentUserProperties)
        {
            _context = context;
            _userManager = userManager;
            _getCurrentUserProperties = getCurrentUserProperties;
        }

        [Authorize(Roles = Roles.Member)]
        public IActionResult MyExercises()
        {//MyExerciseDTO

             //myExer = new List<MyExerciseDTO>();
            var myExer = _context.Exercises
                   .Include(q => q.Member)
                   .ThenInclude(q => q.Person)
                   //.Where(q => q.Member.PersonId == user.Id)//&& q.DateOfNutrition >=DateTime.Now
                   .Where(q => q.Member.PersonId == _getCurrentUserProperties.GetCurrentUserId())
                   //&& q.DateOfNutrition >=DateTime.Now
                   .Select(r => new MyExerciseDTO
                   {
                        Title = r.Title,
                        DateOfExercise =r.DateOfExercise,
                        ExerciseCount=r.ExerciseCount,
                        ExerciseDuration = r.ExerciseDuration,
                        ExerciseFrom= TimeSpan.Parse(ChangeDateFormat.ConvertTimeLocal(r.ExerciseFrom)), 
                        ExerciseTimeFormat= r.ExerciseTimeFormat,
                        ExerciseTO=TimeSpan.Parse(ChangeDateFormat.ConvertTimeLocal(r.ExerciseTO))
                   }).ToList();

            return View(myExer);
        }
        public JsonResult LoadFirst()
        {
            //var result = _context.Nutritions
            //    .Include(q=>q.Member)
            //    .ThenInclude(q=>q.Person)
            //    .Where(q=>q.DateOfNutrition.Date )



            return new JsonResult("");
        }


        [Authorize(Roles = Roles.Member)]
        public IActionResult MyNutrations(string filter="")
        {
            if (!string.IsNullOrEmpty(filter))
            {
                var result = new List<MyNutirationsDTO>();
                result = _context.Nutritions
                .Include(n => n.Member)
                .Include(q => q.Member.Person)
                .Select(r => new MyNutirationsDTO { }).ToList();
                switch (filter.ToLower())
                {
                    case "all":
                        result = _context.Nutritions
                            .Include(n => n.Member)
                            .Include(q => q.Member.Person)
                            .Select(r => new MyNutirationsDTO
                            {
                                MealName = r.MealName,
                                NameOfDay = r.NameOfDay,
                                MemberName = _getCurrentUserProperties.GetCurrentUserEmail(),
                                DateOfNutrition = r.DateOfNutrition,
                                MealType = r.MealType
                            }).ToList();
                        break;
                    case "today":
                        result = _context.Nutritions
                            .Include(n => n.Member)
                            .Include(q => q.Member.Person)
                            .Where(q => q.DateOfNutrition.Date == DateTime.Now.Date)
                            .Select(r => new MyNutirationsDTO
                            {
                                MealName = r.MealName,
                                NameOfDay = r.NameOfDay,
                                MemberName = _getCurrentUserProperties.GetCurrentUserEmail(),
                                DateOfNutrition = r.DateOfNutrition,
                                MealType = r.MealType
                            }).ToList();
                        break;
                    case "finished":
                        result = _context.Nutritions
                            .Include(n => n.Member)
                            .Include(q => q.Member.Person)
                            .Where(q => q.DateOfNutrition < DateTime.Now)
                            .Select(r => new MyNutirationsDTO
                            {
                                MealName = r.MealName,
                                NameOfDay = r.NameOfDay,
                                MemberName = _getCurrentUserProperties.GetCurrentUserEmail(),
                                DateOfNutrition = r.DateOfNutrition,
                                MealType = r.MealType
                            }).ToList();
                        break;
                    case "active":
                        result = _context.Nutritions
                            .Include(n => n.Member)
                            .Include(q => q.Member.Person)
                            .Where(q => q.DateOfNutrition < DateTime.Now)
                            .Select(r => new MyNutirationsDTO
                            {
                                MealName = r.MealName,
                                NameOfDay = r.NameOfDay,
                                MemberName = _getCurrentUserProperties.GetCurrentUserEmail(),
                                DateOfNutrition = r.DateOfNutrition,
                                MealType = r.MealType
                            }).ToList();
                        break;
                    case "notstarted":
                        result = _context.Nutritions
                            .Include(n => n.Member)
                            .Include(q => q.Member.Person)
                            .Where(q => q.DateOfNutrition > DateTime.Now)
                            .Select(r => new MyNutirationsDTO
                            {
                                MealName = r.MealName,
                                NameOfDay = r.NameOfDay,
                                MemberName = _getCurrentUserProperties.GetCurrentUserEmail(),
                                DateOfNutrition = r.DateOfNutrition,
                                MealType = r.MealType
                            }).ToList();
                        break;
                }
                return View(result);
            }
            else
            {
                var myNuti = _context.Nutritions
                    .Include(q => q.Member)
                    .ThenInclude(q => q.Person)
                    //.Where(q => q.Member.PersonId == user.Id)//&& q.DateOfNutrition >=DateTime.Now
                    .Where(q => q.Member.PersonId == _getCurrentUserProperties.GetCurrentUserId() && q.DateOfNutrition.Date == DateTime.Now.Date)
                    //&& q.DateOfNutrition >=DateTime.Now
                    .Select(r => new MyNutirationsDTO
                    {
                        MealName = r.MealName,
                        NameOfDay = r.NameOfDay,
                        MemberName = _getCurrentUserProperties.GetCurrentUserEmail(),
                        DateOfNutrition = r.DateOfNutrition,
                        MealType = r.MealType
                    }).ToList();


                return View(myNuti);

            }


            
        }













        //[Authorize(Roles = Roles.Member)]
        //public IActionResult MyNutrations()
        //{
        //    //var myNuti = new List<MyNutirationsDTO>();
        //    var myNuti = _context.Nutritions
        //        .Include(q => q.Member)
        //        .ThenInclude(q => q.Person)
        //        //.Where(q => q.Member.PersonId == user.Id)//&& q.DateOfNutrition >=DateTime.Now
        //        .Where(q => q.Member.PersonId == _getCurrentUserProperties.GetCurrentUserId() && q.DateOfNutrition.Date == DateTime.Now.Date)
        //        //&& q.DateOfNutrition >=DateTime.Now
        //        .Select(r => new MyNutirationsDTO
        //        {
        //            MealName = r.MealName,
        //            NameOfDay = r.NameOfDay,
        //            MemberName = _getCurrentUserProperties.GetCurrentUserEmail(),
        //            DateOfNutrition = r.DateOfNutrition,
        //            MealType = r.MealType
        //        }).ToList();


        //    return View(myNuti);
        //}



    }
}
