﻿using FitnessApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Models
{
    public class Nutrition
    {

        [Key]
        public string Id { get; set; }
        [Display(Name = "Meal Name")]
        public string MealName { get; set; }
        [ForeignKey("MemberId")]
        [Display(Name = "Member")]
        public string MemberId { get; set; }
        public Member Member { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date Of Nutrition")]
        public DateTime DateOfNutrition { get; set; }
        [Display(Name = "Name Of Day")]
        public string NameOfDay { get; set; }
        [Required]
        [Display(Name = "Meal Type")]
        public MealType MealType { get; set; }

        [ForeignKey("TrainerId")]
        [Display(Name = "Trainer")]
        public string TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        [NotMapped]
        public List<TrainersMember> TrainersMembers { get; }
        
    }

}
