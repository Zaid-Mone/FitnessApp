using FitnessApp.Enums;
using FitnessApp.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace FitnessApp.DTOs
{
    public class MyNutirationsDTO
    {

        [Display(Name = "Meal Name")]
        public string MealName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date Of Nutrition")]
        public DateTime DateOfNutrition { get; set; }
        [Display(Name = "Name Of Day")]
        public string NameOfDay { get; set; }
        [Display(Name = "Member Name")]
        public string MemberName { get; set; }
        [Display(Name = "Meal Type")]
        public MealType MealType { get; set; }
    }
}
