using FitnessApp.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.DTOs
{
    public class InsertNutrationDTO
    {
        [Display(Name = "Meal Name")]
        public string MealName { get; set; }
        [Display(Name = "Member")]
        public string MemberId { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date Of Nutrition")]
        public DateTime DateOfNutrition { get; set; }
        [Display(Name = "Meal Type")]
        public MealType MealType { get; set; }
    }
}
